﻿using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Excess.Compiler;
using Excess.Compiler.Core;
using Excess.Compiler.Attributes;
using Excess.Compiler.Roslyn;

namespace Contract
{
    using CSharp = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    using ExcessCompiler = ICompiler<SyntaxToken, SyntaxNode, SemanticModel>;

    [Extension("contract")]
    public class ContractExtension
    {
        public static void Apply(ExcessCompiler compiler, Scope scope = null)
        {
            scope?.AddKeywords("contract");

            compiler.extension("contract", ParseContract);
        }

        static private Template ContractCheck = Template.ParseStatement(@"
            if (!(__0)) 
                throw new InvalidOperationException(""Breach of contract!!"");");

        private static SyntaxNode ParseContract(BlockSyntax block, Scope scope)
        {
            List<StatementSyntax> checks = new List<StatementSyntax>();
            foreach (var st in block.Statements)
            {
                var stExpression = st as ExpressionStatementSyntax;
                if (stExpression == null)
                {
                    scope.AddError("contract01", "contracts only support boolean expressions", st);
                    continue;
                }

                checks.Add(ContractCheck
                    .Get<StatementSyntax>(stExpression.Expression));
            }

            return CSharp.Block(checks);
        }
    }
}