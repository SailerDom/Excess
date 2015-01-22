﻿using Excess.Compiler.Core;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excess.Compiler.Roslyn
{
    public class SyntacticalMatchResult : BaseSyntacticalMatchResult<SyntaxNode>
    {
        public SyntacticalMatchResult(Scope scope, IEventBus events, SyntaxNode node = null)
            : base(node, scope, events)
        {
            Preprocess = false;
        }

        protected override IEnumerable<SyntaxNode> children(SyntaxNode node)
        {
            return node.ChildNodes();
        }

        protected override IEnumerable<SyntaxNode> descendants(SyntaxNode node)
        {
            return node.DescendantNodes();
        }
        protected override SyntaxNode markNode(SyntaxNode node, int id)
        {
            return RoslynCompiler.SetSyntaxId(node, id);
        }
    }

    public class RoslynSyntaxAnalysis : SyntaxAnalysisBase<SyntaxNode>
    {
    }
}
