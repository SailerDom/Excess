﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Excess.Compiler;
using Excess.Compiler.Core;

using CSharp = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Excess.RuntimeProject
{
    using Injector = ICompilerInjector<SyntaxToken, SyntaxNode, SemanticModel>;
    using DelegateInjector = DelegateInjector<SyntaxToken, SyntaxNode, SemanticModel>;
    using CompositeInjector = CompositeInjector<SyntaxToken, SyntaxNode, SemanticModel>;

    class ConsoleRuntime : BaseRuntime
    {
        public override string defaultFile()
        {
            return "application";
        }

        private static Injector _main = new DelegateInjector(compiler =>
        {
            compiler
                .Lexical()
                    .normalize()
                        .with(statements: MoveToMain, members: MoveToApplication);
        });

        private static SyntaxNode MoveToMain(SyntaxNode root, IEnumerable<SyntaxNode> statements, Scope scope)
        {
            var mainMethod = CSharp.MethodDeclaration(CSharp.ParseTypeName("void"), "main")
                                .WithBody(CSharp.Block()
                                    .WithStatements(CSharp.List(statements)));

            return (root as CompilationUnitSyntax).AddMembers(mainMethod);
        }

        private static SyntaxNode MoveToApplication(SyntaxNode root, IEnumerable<SyntaxNode> members, Scope scope)
        {
            var appClass = CSharp.ClassDeclaration("application")
                .WithMembers(CSharp.List(members));

            return CSharp.CompilationUnit()
                .WithMembers(CSharp.List( new[] {
                        (MemberDeclarationSyntax)appClass
                }));
        }

        protected override ICompilerInjector<SyntaxToken, SyntaxNode, SemanticModel> getInjector(string file)
        {
            var xs = base.getInjector(file);
            if (file == "application")
                return new CompositeInjector(new[] { _main, xs });

            return xs;
        }

        protected override void doRun(Assembly asm, out dynamic clientData)
        {
            clientData = null;

            Type appType = asm.GetType("application");
            if (appType == null)
            {
                notify(NotificationKind.Error, "Application class missing");
                return;
            }

            var main = appType.GetMethod("main", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (main == null)
            {
                notify(NotificationKind.Error, "Main method missing");
                return;
            }

            var instance = FormatterServices.GetUninitializedObject(appType);// Activator.CreateInstance(appType);
            main.Invoke(instance, new object[] { });
        }
    }
}
