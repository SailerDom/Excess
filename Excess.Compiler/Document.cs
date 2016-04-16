﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excess.Compiler
{
    public interface IDocument<TToken, TNode, TModel>
    {
        void change(Func<IEnumerable<TToken>, Scope, IEnumerable<TToken>> transform, string kind = null);
        TToken change(TToken token, Func<TNode, Scope, TNode> transform, string kind = null);
        IEnumerable<TToken> change(IEnumerable<TToken> tokens, Func<TNode, Scope, TNode> transform, string kind = null);
        TToken change(TToken token, Func<TNode, TNode, TModel, Scope, TNode> transform, string kind = null);
        IEnumerable<TToken> change(IEnumerable<TToken> tokens, Func<TNode, TNode, TModel, Scope, TNode> transform, string kind = null);
        void change(Func<TNode, Scope, TNode> transform, string kind = null);
        TNode change(TNode node, Func<TNode, Scope, TNode> transform, string kind = null);
        void change(Func<TNode, TModel, Scope, TNode> transform, string kind = null);
        TNode change(TNode node, Func<TNode, TNode, TModel, Scope, TNode> transform, string kind = null);

        bool applyChanges();
        bool applyChanges(CompilerStage stage);
        bool hasErrors();
        bool HasSemanticalChanges();

        string Text { get; set; }
        CompilerStage Stage { get; }
        TNode SyntaxRoot { get; set; }
        TModel Model { get; set; }
        Scope Scope { get; }
        IMappingService<TToken, TNode> Mapper { get; set; }
    }

    public interface IDocumentInjector<TToken, TNode, TModel>
    {
        void apply(IDocument<TToken, TNode, TModel> document);
    }

    //Helper class to be used in a similar role as Roslyn's TextSpan
    public class SourceSpan
    {
        public SourceSpan()
        {
        }

        public SourceSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public int Start { get; internal set; }
        public int Length { get; internal set; }
    }

    public interface IMappingService<TToken, TNode>
    {
        TNode LexicalTree { get; set; }
        TNode SyntacticalTree { get; set; }
        TNode SemanticalTree { get; set; }

        void LexicalChange(SourceSpan oldSpan, int newLength);
        void SemanticalChange(TNode oldNode, TNode newNode);

        TNode SemanticalMap(SourceSpan src);
        TNode SemanticalMap(TNode node);
        SourceSpan SourceMap(TNode node);

        TToken Transform(TToken token);
        string MapLines(TNode node, string fileName);
        TNode AppyMappings(TNode root, Dictionary<int, string> mappings);
    }
}
