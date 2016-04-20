﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Excess.Compiler;
using Excess.Compiler.Roslyn;
using Excess.Extensions.Concurrent.Model;
using Excess.Concurrent.Runtime;

namespace Excess.Extensions.Concurrent
{
    using CSharp = SyntaxFactory;
    using Roslyn = RoslynCompiler;

    public class Options
    {
        public Options()
        {
            GenerateID = true; //td: tidy up
            BlockUntilNextEvent = true;
            ThreadCount = 4;
            GenerateAppConstructor = true;
        }

        public int ThreadCount { get; set; }
        public bool BlockUntilNextEvent { get; set; }
        public bool AsFastAsPossible { get; set; }
        public bool GenerateInterface { get; set; }
        public bool GenerateRemote { get; set; }
        public bool GenerateID { get; set; }
        public bool GenerateAppConstructor { get; internal set; }
    }

    public class Extension
    {
        public static IEnumerable<string> GetKeywords()
        {
            return new[] { "concurrent", "spawn" };
        }

        public static void Apply(RoslynCompiler compiler, Options options = null)
        {
            if (options == null)
                options = new Options();

            var lexical = compiler.Lexical();
            lexical
                .match()
                    .token("concurrent", named: "keyword")
                    .token("class", named: "ref")
                    .then(lexical.transform()
                        .remove("keyword")
                        .then(CompileClass(options)))

                .match()
                    .token("concurrent", named: "keyword")
                    .token("object", named: "ref")
                    .then(lexical.transform()
                        .remove("keyword")
                        .replace("ref", "class ")
                        .then(CompileObject(options)))

                .match()
                    .token("concurrent", named: "keyword")
                    .token("app", named: "ref")
                    .token("{")
                    .then(lexical.transform()
                        .replace("keyword", "class ")
                        .replace("ref", "__app")
                        .then(CompileApp(options)));

            compiler.Environment()
                .dependency(new[]
                {
                    "System.Threading",
                    "System.Threading.Tasks",
                })
                .dependency<ConcurrentObject>("Excess.Concurrent.Runtime");
        }

        private static Func<SyntaxNode, Scope, SyntaxNode> CompileClass(Options options)
        {
            return (node, scope) => Compile(node, scope, false, options);
        }

        private static SyntaxNode Compile(SyntaxNode node, Scope scope, bool isSingleton, Options options)
        {
            Debug.Assert(node is ClassDeclarationSyntax);
            var @class = (node as ClassDeclarationSyntax)
                .AddBaseListTypes(
                    CSharp.SimpleBaseType(CSharp.ParseTypeName(
                        "ConcurrentObject")));

            if (options.GenerateInterface)
            {
                @class = @class.AddBaseListTypes(
                    CSharp.SimpleBaseType(CSharp.ParseTypeName(
                        "I" + (node as ClassDeclarationSyntax).Identifier.ToString())));
            }

            var className = @class.Identifier.ToString();

            var ctx = new Class(className, scope, isSingleton);
            scope.set<Class>(ctx);

            foreach (var member in @class.Members)
            {
                if (member is PropertyDeclarationSyntax)
                    compileProperty(member as PropertyDeclarationSyntax, ctx, scope);
                else if (member is MethodDeclarationSyntax)
                {
                    var method = member as MethodDeclarationSyntax;
                    if (compileMethod(method, ctx, scope))
                    {
                        var isVoid = method.ReturnType.ToString() == "void";
                        var taskArgs = isVoid
                            ? new[]
                            {
                                CSharp.Argument(Templates.NullCancelationToken),
                                CSharp.Argument(Roslyn.@null),
                                CSharp.Argument(Roslyn.@null)
                            }
                            : new[]
                            {
                                CSharp.Argument(Templates.NullCancelationToken)
                            };

                        var taskCall = CSharp
                            .InvocationExpression(
                                CSharp.IdentifierName(method.Identifier),
                                CSharp.ArgumentList(CSharp.SeparatedList(
                                    method
                                    .ParameterList
                                    .Parameters
                                    .Select(parameter => CSharp
                                        .Argument(CSharp.IdentifierName(parameter.Identifier)))
                                    .Union(
                                        taskArgs))));

                        ctx.Replace(method, method
                            .AddAttributeLists(CSharp.AttributeList(CSharp.SeparatedList(new[] {CSharp
                                .Attribute(CSharp
                                    .ParseName("Concurrent"))})))
                            .WithBody(CSharp.Block()
                                .WithStatements(CSharp.List(new[] {
                                    isVoid
                                        ? Templates
                                            .SynchVoidMethod
                                            .Get<StatementSyntax>(taskCall)
                                        : Templates
                                            .SynchReturnMethod
                                            .Get<StatementSyntax>(taskCall)}))));
                    }
                }
            }

            //all concurrent compilation has been done, produce 
            @class = ctx.Update(@class);

            var document = scope.GetDocument();

            //generate the interface
            if (options.GenerateInterface)
                document.change(node.Parent, Roslyn.AddMember(CreateInterface(@class)));

            if (options.GenerateRemote)
            {
                //add a remote type, to be used with an identity server  
                var remoteMethod = null as MethodDeclarationSyntax;
                var remoteType = createRemoteType(@class, out remoteMethod);
                @class = @class.AddMembers(
                    remoteMethod,
                    remoteType);
            }

            if (options.GenerateID)
                @class = @class.AddMembers(Templates
                    .ObjectId
                    .Get<MemberDeclarationSyntax>());

            //schedule linking
            return document.change(@class, Link(ctx), null);
        }

        private static IEnumerable<MethodDeclarationSyntax> getConcurrentMethods(ClassDeclarationSyntax @class)
        {
            return @class
                .ChildNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(method => method.Identifier.ToString() != "__concurrentmain"
                              && isInternalConcurrent(method));
        }

        private static IEnumerable<MemberDeclarationSyntax> getConcurrentInterface(ClassDeclarationSyntax @class)
        {
            return @class
                .ChildNodes()
                .OfType<MemberDeclarationSyntax>()
                .Where(member =>
                {
                    if (member is MethodDeclarationSyntax && Roslyn.IsVisible(member))
                        return !(member as MethodDeclarationSyntax)
                            .Modifiers
                            .Any(modifier => modifier.Kind() == SyntaxKind.StaticKeyword);

                    return false;
                });
        }

        private static MemberDeclarationSyntax CreateInterface(ClassDeclarationSyntax @class)
        {
            var concurrentInterface = getConcurrentInterface(@class);

            return Templates
                .Interface
                .Get<InterfaceDeclarationSyntax>("I" + @class.Identifier.ToString())
                .AddMembers(concurrentInterface
                    .Select(method => interfaceMethod(method as MethodDeclarationSyntax))
                    .ToArray());
        }

        private static MemberDeclarationSyntax interfaceMethod(MethodDeclarationSyntax method)
        {
            return method
                .WithAttributeLists(CSharp.List<AttributeListSyntax>())
                .WithModifiers(CSharp.TokenList())
                .WithBody(null)
                .WithSemicolonToken(CSharp.ParseToken(";"));
        }

        private static ClassDeclarationSyntax createRemoteType(ClassDeclarationSyntax @class, out MethodDeclarationSyntax creation)
        {
            var originalName = @class.Identifier.ToString();
            var typeName = "__remote" + originalName;

            creation = Templates
                .RemoteMethod
                .Get<MethodDeclarationSyntax>(typeName, "I" + originalName);

            return @class
                .WithIdentifier(CSharp.ParseToken(typeName))
                .WithAttributeLists(CSharp.List<AttributeListSyntax>())
                .WithMembers(CSharp.List<MemberDeclarationSyntax>(
                    getConcurrentInterface(@class)
                    .Union(getConcurrentMethods(@class)
                        .Select(mm => createRemoteMethod(mm)))
                    .Union(new[] {
                        Templates.RemoteId,
                        Templates.RemoteDispatch,
                        Templates.RemoteSerialize,
                        Templates.RemoteDeserialize
                    })));
        }

        private static bool isInternalConcurrent(MethodDeclarationSyntax method)
        {
            var methodName = method
                .Identifier
                .ToString();

            return methodName.StartsWith("__concurrent");
        }

        private static MethodDeclarationSyntax createRemoteMethod(MethodDeclarationSyntax method)
        {
            var original = method
                .Parent
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(m => 
                    method.Identifier.ToString() == "__concurrent" + m.Identifier.ToString() 
                    && m.AttributeLists
                        .Any(attrList => attrList
                            .Attributes
                            .Any(attr => attr.Name.ToString() == "Concurrent")))
                .Single();

            var args = CSharp
                .AnonymousObjectCreationExpression(CSharp.SeparatedList(method
                    .ParameterList
                    .Parameters
                    .Select(parameter =>
                    {
                        var identifierName = CSharp.IdentifierName(parameter.Identifier);
                        return CSharp.AnonymousObjectMemberDeclarator(
                            CSharp.NameEquals(identifierName),
                            identifierName);
                    })));

            var value = original
                .ReturnType.ToString() == "void"
                ? Roslyn.@null
                : Templates
                    .RemoteResult
                    .Get<ExpressionSyntax>(original.ReturnType);

            return method
                .WithBody(Templates
                    .RemoteInternalMethod
                    .Get<BlockSyntax>(
                        Roslyn.Quoted(original.Identifier.ToString()),
                        args,
                        value));
        }

        private static Func<SyntaxNode, SyntaxNode, SemanticModel, Scope, SyntaxNode> Link(Class ctx)
        {
            return (oldNode, newNode, model, scope) =>
            {
                if (newNode is ClassDeclarationSyntax)
                {
                    var @class = new ExpressionLinker(ctx, model)
                        .Visit(newNode);

                    Debug.Assert(@class != null);
                    Debug.Assert(@class is ClassDeclarationSyntax);
                    return ctx.Update(@class as ClassDeclarationSyntax);
                }

                return newNode;
            };
        }

        private static void compileProperty(PropertyDeclarationSyntax property, Class ctx, Scope scope)
        {
            if (!Roslyn.IsVisible(property))
                return;

            var @get = null as AccessorDeclarationSyntax;
            var @set = null as AccessorDeclarationSyntax;
            foreach (var accesor in property.AccessorList.Accessors)
            {
                switch (accesor.Keyword.Kind())
                {
                    case SyntaxKind.GetKeyword:
                        @get = accesor;
                        break;
                    case SyntaxKind.SetKeyword:
                        @set = accesor;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            bool hasCustomGet = @get != null && @get.Body != null && @get.Body.Statements.Count > 0;
            if (hasCustomGet && @get.Body.Statements.Count == 1)
                hasCustomGet = !(@get.Body.Statements[0] is ReturnStatementSyntax);

            bool hasCustomSet = @set != null && @set.Body != null && @set.Body.Statements.Count > 0;
            if (hasCustomSet && @set.Body.Statements.Count == 1)
                hasCustomSet = !(@set.Body.Statements[0] is ExpressionStatementSyntax)
                            || (@set.Body.Statements[0] as ExpressionStatementSyntax)
                                    .Expression.Kind() != SyntaxKind.SimpleAssignmentExpression;

            if (hasCustomGet || hasCustomSet)
            {
                scope.AddError("concurrent00", "invalid concurrent property, custom accessors are not allowed", property);
            }
        }

        private static bool compileMethod(MethodDeclarationSyntax methodDeclaration, Class ctx, Scope scope)
        {
            var method = methodDeclaration;
            var name = method.Identifier.ToString();
            var isMain = name == "main";

            var isProtected = method
                .Modifiers
                .Where(m => m.Kind() == SyntaxKind.ProtectedKeyword)
                .Any();

            var isVisible = isProtected || Roslyn.IsVisible(method);

            var hasReturnType = method.ReturnType.ToString() != "void";
            var returnType = hasReturnType 
                ? method.ReturnType
                : Roslyn.boolean;

            var isEmptySignal = method.Body == null 
                             || method.Body.IsMissing;
            if (isEmptySignal)
            {
                if (method.ParameterList.Parameters.Count > 0)
                    scope.AddError("concurrent03", "empty signals cannot contain parameters", method);

                if (method.ReturnType.ToString() != "void")
                    scope.AddError("concurrent04", "empty signals cannot return values", method);

                method = method
                    .WithSemicolonToken(CSharp.MissingToken(SyntaxKind.SemicolonToken))
                    .WithBody(CSharp.Block());
            }

            var cc = parseConcurrentBlock(ctx, method.Body, scope);
            if (cc != null)
                method = method.WithBody(cc);

            //remove attributes, until needed
            method = method.WithAttributeLists(CSharp.List<AttributeListSyntax>());

            if (isMain)
            {
                if (ctx.HasMain)
                {
                    scope.AddError("concurrent06", "multiple main methods", method);
                    return false;
                }

                ctx.HasMain = true;

                var statements = method.Body.Statements;
                var isContinued = (cc == null) && checkContinued(statements);
                if (isContinued)
                {
                    method = method
                        .WithBody(CSharp.Block(statements
                            .Take(statements.Count - 1)));
                }

                var mainMethod = concurrentMethod(ctx, method);

                //hook up our start method
                int currentIndex = 0;
                ctx.Replace(methodDeclaration, Templates
                    .StartObject
                    .Get<MethodDeclarationSyntax>(Templates
                        .ConcurrentMain
                        .Get<InvocationExpressionSyntax>()
                        .WithArgumentList(CSharp.ArgumentList(CSharp.SeparatedList(
                            mainMethod
                            .ParameterList
                            .Parameters
                            .Where(param => param.Identifier.ToString() != "__cancellation"
                                         && param.Identifier.ToString() != "__success"
                                         && param.Identifier.ToString() != "__failure")
                            .Select(param => CSharp.Argument(Templates
                                .StartObjectArgument
                                .Get<ExpressionSyntax>(
                                    param.Type,
                                    currentIndex++)))
                             .Union(new[] {
                                 CSharp.Argument(Templates.NullCancelationToken),
                                 CSharp.Argument(Roslyn.@null),
                                 CSharp.Argument(Roslyn.@null),
                             }))))));

                return false;
            }

            if (isVisible)
            {
                if (isEmptySignal)
                    ctx.AddMember(Templates
                        .EmptySignalMethod
                        .Get<MethodDeclarationSyntax>(
                            "__concurrent" + name, 
                            Roslyn.Quoted(name),
                            isProtected ? Roslyn.@true : Roslyn.@false));
                else
                    concurrentMethod(ctx, method);
            }
            else if (cc != null)
                concurrentMethod(ctx, method);
            else if (isEmptySignal)
                ctx.AddMember(Templates
                    .EmptySignalMethod
                    .Get<MethodDeclarationSyntax>(
                        "__concurrent" + name, 
                        Roslyn.Quoted(name),
                        isProtected? Roslyn.@true : Roslyn.@false));
            else
                return false;

            var signal = ctx.AddSignal(name, returnType, isVisible);
            if (isVisible)
                createPublicSignals(ctx, method, signal);
            else
            {
                ctx.Replace(methodDeclaration, method
                    .WithBody(CSharp.Block()
                        .WithStatements(CSharp.List(new[] {
                            Templates.PrivateSignal
                        }))));
                return false;
            }

            return true;
        }

        private static BlockSyntax parseConcurrentBlock(Class ctx, BlockSyntax body, Scope scope)
        {
            var rewriter = new ExpressionParser(ctx, scope);
            var result = (BlockSyntax)rewriter.Visit(body);

            if (rewriter.HasConcurrent)
                return result;

            return null;
        }

        private static MethodDeclarationSyntax concurrentMethod(Class ctx, MethodDeclarationSyntax method, bool forever = false)
        {
            var name = method.Identifier.ToString();

            var body = method.Body;
            var returnStatements = body
                .DescendantNodes()
                .OfType<ReturnStatementSyntax>();

            var lastReturn = body.Statements.LastOrDefault() as ReturnStatementSyntax;

            if (returnStatements.Any())
                body = body
                    .ReplaceNodes(returnStatements, 
                    (on, nn) => Templates
                        .ExpressionReturn
                        .Get<StatementSyntax>(nn.Expression == null || nn.Expression.IsMissing
                            ? Roslyn.@null
                            : nn.Expression,
                            Roslyn.Quoted(method.Identifier.ToString())));

            if (forever)
            {
                body = CSharp.Block(
                    CSharp.ForStatement(body));
            }
            else if (lastReturn == null)
                body = body.AddStatements(Templates
                    .ExpressionReturn
                    .Get<StatementSyntax>(
                        Roslyn.@null,
                        Roslyn.Quoted(method.Identifier.ToString())));

            var result = Templates
                .ConcurrentMethod
                .Get<MethodDeclarationSyntax>("__concurrent" + name);
            result = result
                .WithParameterList(method
                    .ParameterList
                    .AddParameters(result
                        .ParameterList
                        .Parameters
                        .ToArray()))
                .WithBody(body);

            ctx.AddMember(result);
            return result;
        }

        private static bool checkContinued(IEnumerable<StatementSyntax> statements)
        {
            var statement = statements.LastOrDefault();
            return (statement != null && statement is ContinueStatementSyntax);
        }

        private static void createPublicSignals(Class ctx, MethodDeclarationSyntax method, Signal signal)
        {
            var returnType = method.ReturnType.ToString() != "void"
                ? method.ReturnType
                : Roslyn.@object;

            var internalMethod = method.Identifier.ToString();
            if (!internalMethod.StartsWith("__concurrent"))
                internalMethod = "__concurrent" + internalMethod;

            var internalCall = Templates
                .InternalCall
                .Get<ExpressionSyntax>(internalMethod);

            internalCall = internalCall
                .ReplaceNodes(internalCall
                    .DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Where(i => i.ArgumentList.Arguments.Count == 3),
                (on, nn) => nn
                    .WithArgumentList(CSharp.ArgumentList(CSharp.SeparatedList(
                        method.ParameterList.Parameters
                            .Select(param => CSharp.Argument(CSharp.IdentifierName(
                                param.Identifier)))
                        .Union(on.ArgumentList.Arguments)))));

            ctx.AddMember(AddParameters(
                method.ParameterList,
                Templates.TaskPublicMethod
                .Get<MethodDeclarationSyntax>(
                    method.Identifier.ToString(),
                    returnType,
                    internalCall)));

            ctx.AddMember(AddParameters(
                method.ParameterList,
                Templates.TaskCallbackMethod
                .Get<MethodDeclarationSyntax>(
                    method.Identifier.ToString(),
                    internalCall)));
        }

        private static MemberDeclarationSyntax AddParameters(ParameterListSyntax parameterList, MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax
            .WithParameterList(methodDeclarationSyntax.ParameterList
                .WithParameters(CSharp.SeparatedList(
                    parameterList.Parameters
                    .Union(methodDeclarationSyntax
                        .ParameterList.Parameters))));
        }

        private static Func<SyntaxNode, Scope, SyntaxNode> CompileObject(Options options)
        {
            return (node, scope) =>
            {
                var @class = node as ClassDeclarationSyntax;
                Debug.Assert(@class != null);
                if (@class
                    .DescendantNodes()
                    .OfType<ConstructorDeclarationSyntax>()
                    .Any())
                {
                    //td: error
                    return node;
                }

                return Compile(node, scope, true, options);
            };
        }

        private static Func<SyntaxNode, Scope, SyntaxNode> CompileApp(Options options)
        {
            var compileObject = CompileObject(options);
            return (node, scope) =>
            {
                var result = (ClassDeclarationSyntax)node;
                var main = result
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(method => method.Identifier.ToString() == "main")
                    .SingleOrDefault();

                if (main != null)
                    result = result.ReplaceNode(main, CompileAppMain(main, options));
                else
                    Debug.Assert(false, "concurrent app must have main"); //td: error

                //convert to concurrent object
                result = (ClassDeclarationSyntax)compileObject(result, scope);

                //add a way for this app to run, stop and await completion
                var runner = null as Func<BlockSyntax, MemberDeclarationSyntax>;
                if (options.GenerateAppConstructor)
                    runner = body => Templates.AppConstructor.WithBody(body);
                else
                    runner = body => Templates.AppRun.WithBody(body); 

                return result.AddMembers(
                    runner(Templates.AppThreaded
                        .Get<BlockSyntax>(
                            Roslyn.Constant(options.ThreadCount),
                            Roslyn.Constant(options.BlockUntilNextEvent),
                            options.AsFastAsPossible
                                ? Templates.HighPriority
                                : Templates.NormalPriority)),
                    Templates.AppStop,
                    Templates.AppAwait);
            };
        }

        private static MethodDeclarationSyntax CompileAppMain(MethodDeclarationSyntax main, Options options)
        {
            foreach (var parameter in main.ParameterList.Parameters)
            {
                if (parameter.Default != null)
                {
                    switch (parameter.Identifier.ToString())
                    {
                        case "threads":
                            var value = parameter
                                ?.Default
                                .Value;

                            Debug.Assert(value != null); //td: error
                            options.ThreadCount = int.Parse(value.ToString());
                            break;
                    }
                }
            }

            return main
                .WithParameterList(Templates.AppMainParameters)
                .WithBody(main.Body
                    .AddStatements(CSharp.ReturnStatement()));
        }
    }
}
