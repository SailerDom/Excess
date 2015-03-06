﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excess.Compiler.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

using Excess.Extensions;

namespace Excess.Compiler.Tests
{
    [TestClass]
    public class ExtUsage
    {
        [TestMethod]
        public void Match()
        {
            RoslynCompiler compiler = new RoslynCompiler();
            Extensions.Match.Apply(compiler);

            SyntaxTree tree = null;
            string text = null;

            //event handler usage
            var SimpleUsage = @"
                class foo
                {
                    void bar()
                    {
                        match(x)
                        {
                            case 10: is_10();
                            case > 10: greater_than_10();
                            default: less_than_10();
                        }
                    }
                }";

            tree = compiler.ApplySemanticalPass(SimpleUsage, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Count() == 2); //must have replaced the match with 2 ifs

            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<ElseClauseSyntax>()
                .Count() == 2); //must have added an else for the default case

            var MultipleUsage = @"
                class foo
                {
                    void bar()
                    {
                        match(x)
                        {
                            case 10: 
                            case 20: is_10_or_20();
                            case > 10: 
                                greater_than_10();
                                greater_than_10();
                            
                            case < 10: 
                            default: less_than_10();
                        }
                    }
                }";

            tree = compiler.ApplySemanticalPass(MultipleUsage, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<IfStatementSyntax>()
                .First()
                    .DescendantNodes()
                    .OfType<BlockSyntax>()
                    .Count() == 1); //must have added a block for multiple stements

            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Where(expr => expr.OperatorToken.CSharpKind() == SyntaxKind.BarBarToken)
                .Count() == 1); //must have added an or expression for multiple cases, 
                                //but not on the case containing the default statement
        }

        [TestMethod]
        public void Asynch()
        {
            RoslynCompiler compiler = new RoslynCompiler();
            Extensions.Asynch.Apply(compiler);

            SyntaxTree tree = null;
            string text = null;

            //event handler usage
            var Asynch = @"
                class foo
                {
                    void bar()
                    {
                        asynch()
                        {
                            foobar();
                        }
                    }
                }";

            tree = compiler.ApplySemanticalPass(Asynch, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<ParenthesizedLambdaExpressionSyntax>()
                .Count() == 1); //must have added a callback 

            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(invocation => invocation.Expression.ToString() == "Task.Factory.StartNew")
                .Count() == 1); //must have added a task factory invocation

            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<LocalDeclarationStatementSyntax>()
                .Count() == 1); //must have added a local variable for the asynch context

            var Synch = @"
                class foo
                {
                    void bar()
                    {
                        asynch()
                        {
                            foobar();
                            synch()
                            {
                                barfoo();
                            }
                        }
                    }
                }";

            tree = compiler.ApplySemanticalPass(Synch, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<ParenthesizedLambdaExpressionSyntax>()
                .Count() == 2); //must have added a callback for asynch and another for synch
        }

        [TestMethod]
        public void Contract()
        {
            RoslynCompiler compiler = new RoslynCompiler();
            Extensions.Contract.Apply(compiler);

            SyntaxTree tree = null;
            string text = null;

            //usage
            var Usage = @"
                class foo
                {
                    void bar(int x, object y)
                    {
                        contract()
                        {
                            x > 3;
                            y != null;
                        }
                    }
                }";

            tree = compiler.ApplySemanticalPass(Usage, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Count() == 2); //must have added an if for each contract condition

            //errors
            var Errors = @"
                class foo
                {
                    void bar(int x, object y)
                    {
                        contract()
                        {
                            x > 3;
                            return 4; //contract01 - only expressions
                        }

                        var noContract = contract() //contract01 - not as expression
                        {
                            x > 3;
                            return 4; 
                        }
                    }
                }";

            var doc = compiler.CreateDocument(Errors);
            compiler.ApplySemanticalPass(doc, out text);
            Assert.IsTrue(doc
                .GetErrors()
                .Count() == 2); //must produce 2 errors
        }

        [TestMethod]
        public void RUsage()
        {
            RoslynCompiler compiler = new RoslynCompiler();
            Excess.Extensions.R.Extension.Apply(compiler);

            SyntaxTree tree = null;
            string text = null;

            //usage
            var Vectors = @"
                void main()
                {
                    R()
                    {
                        x <- c(10.4, 5.6, 3.1, 6.4, 21.7)
                        y <- c(x, 0, x)
                        z <- 2*x + y + 1

                        a <- x > 13
                        b <- x[!(is.na(x))]
                        c <- x[-(1:5)]
                    }
                }";

            tree = compiler.ApplySemanticalPass(Vectors, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Count() == 6); //must have created 5 variables (x, y, z, a, b)

            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count() == 0); //must have replaced all operators

            var Sequences = @"
                void main()
                {
                    R()
                    {
                        x <- 1:30
                        y <- 2*1:15
                        seq(-5, 5, by=.2) -> s3
                        s4 <- seq(length=51, from=-5, by=.2)
                        s5 <- rep(x, times=5)
                    }
                }";

            tree = compiler.ApplySemanticalPass(Sequences, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(invocation => invocation.Expression.ToString().Contains("RR"))
                .Count() == 6); //must have replaced all operators

            var Statements = @"
                void main()
                {
                    R()
                    {
                        x <- 1
                        y <- 2
                        z <- NA

                        if (x == 1) 
                            3 -> z
                                                    
                        if (y == 1) 
                        {
                            3 -> z
                        }
                        else
                        {
                            z1 <- 4
                            z <- z1 
                        }

                        while(z < 10)  c(a, z) -> a

                        for(i in z)
                        {
                            a <- c(a, i);
                        }

                        repeat
                        {
                            b <- a   
                            a <- c(b, b);
                            if (length(a) > 10) break;
                        }
                    }
                }";

            tree = compiler.ApplySemanticalPass(Statements, out text);
            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<WhileStatementSyntax>()
                .Count() == 2); //must have replaced a while and a repeat

            Assert.IsTrue(tree.GetRoot()
                .DescendantNodes()
                .OfType<StatementSyntax>()
                .Where(ss => !(ss is ExpressionStatementSyntax || ss is LocalDeclarationStatementSyntax || ss is BlockSyntax))
                .Count() == 7); //3 if, 2 whiles, a foreach, a break
        }
    }
}