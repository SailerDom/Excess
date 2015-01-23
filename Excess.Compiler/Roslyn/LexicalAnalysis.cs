﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Excess.Compiler.Core;

namespace Excess.Compiler.Roslyn
{
    using Microsoft.CodeAnalysis.CSharp;
    using CSharp = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    public class RoslynLexicalTransform : LexicalTransform<SyntaxToken>
    {
        protected override IEnumerable<SyntaxToken> tokensFromString(string tokenString)
        {
            return RoslynCompiler.ParseTokens(tokenString);
        }
    }

    public class RoslynLexicalMatch : BaseLexicalMatch<SyntaxToken, SyntaxNode>
    {
        public RoslynLexicalMatch(ILexicalAnalysis<SyntaxToken, SyntaxNode> lexical) :
            base(lexical)
        {
        }

        protected override bool isIdentifier(SyntaxToken token)
        {
            return token.IsVerbatimIdentifier();
        }
    }

    public class RoslynLexicalAnalysis : LexicalAnalysis<SyntaxToken, SyntaxNode>
    {
        public override ILexicalTransform<SyntaxToken> transform()
        {
            return new RoslynLexicalTransform();
        }

        protected override ILexicalMatch<SyntaxToken, SyntaxNode> createMatch()
        {
            return new RoslynLexicalMatch(this);
        }

        public override IEnumerable<SyntaxToken> parseTokens(string tokenString)
        {
            var tokens = CSharp.ParseTokens(tokenString);
            foreach (var token in tokens)
            {
                if (token.CSharpKind() != SyntaxKind.EndOfFileToken)
                    yield return token;
            }
        }

        protected override SyntaxToken setLexicalId(SyntaxToken token, int value)
        {
            return RoslynCompiler.SetLexicalId(token, value);
        }
    }
}
