﻿using Excess.Compiler;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excess.Compiler.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Excess.Extensions.Model
{
    using ExcessCompiler = ICompiler<SyntaxToken, SyntaxNode, SemanticModel>;
    using CSharp = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    using Microsoft.CodeAnalysis.CSharp;
    using Compiler.Roslyn;
    [Extension("model")]
    public class ModelExtension
    {
        public static void Apply(ExcessCompiler compiler, Scope scope = null)
        {
            scope?.AddKeywords("injector");

            var lexical = compiler.Lexical();
            lexical
                .match()
                    .token("model", named: "keyword")
                    .identifier()
                    .enclosed('{', '}', end: "lastBrace")
                    .then(lexical.transform()
                        .replace("keyword", "class ")
                        .then(ParseModel));

            //td: !!! .extension("model", ExtensionKind.Type, ParseModel);
        }

        private static Template ModelProperty = Template.Parse("public __0 _1 { get; private set; }");

        private static SyntaxNode ParseModel(SyntaxNode node, Scope scope)
        {
            var @class = (ClassDeclarationSyntax)node;
            var init = new List<ParameterSyntax>();
            var props = new List<PropertyDeclarationSyntax>();
            foreach (var member in @class.Members)
            {
                var field = ParseField(member);
                if (field == null)
                    continue; //error has already been registered

                var type = field.Declaration.Type;
                var variable = field.Declaration
                    .Variables
                    .Single();


                init.Add(CSharp.Parameter(variable.Identifier)
                    .WithType(type)
                    .WithDefault(CSharp.EqualsValueClause(
                        variable.Initializer != null
                            ? variable.Initializer.Value
                            : CSharp.DefaultExpression(type))));

                props.Add(ModelProperty.Get<PropertyDeclarationSyntax>(type, variable.Identifier));
            }

            return CSharp.StructDeclaration(@class.Identifier)
                .AddMembers(GenerateConstructor(@class, init))
                .AddMembers(props.ToArray());
        }

        private static Template ModelAssign = Template.ParseStatement(
            "this._0 = _0;");

        private static MemberDeclarationSyntax GenerateConstructor(ClassDeclarationSyntax @class, IEnumerable<ParameterSyntax> parameters)
        {
            return CSharp.ConstructorDeclaration(@class.Identifier)
                .WithModifiers(RoslynCompiler.@public)
                .AddParameterListParameters(parameters.ToArray())
                .AddBodyStatements(parameters
                    .Select(param => 
                        ModelAssign.Get<StatementSyntax>(param.Identifier))
                    .ToArray());
        }

        private static FieldDeclarationSyntax ParseField(MemberDeclarationSyntax member)
        {
            var result = member as FieldDeclarationSyntax;
            if (result == null)
            {
                //td: error
                return null;
            }

            if (result.Modifiers.Any())
            {
                //td: error, no modifiers
                return null;
            }

            return result;
        }
    }
}
