﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excess.Compiler
{
    public class CompilerEvent
    {
        public CompilerEvent(CompilerStage stage, string pass)
        {
            Stage = stage;
            Pass = pass;
        }

        public CompilerStage Stage { get; }
        public string Pass { get; set; }
    }

    public interface IEventBus
    {
        IEnumerable<CompilerEvent> poll(string pass);
        IEnumerable<T> poll<T>() where T : CompilerEvent;
        IEnumerable<CompilerEvent> check(string pass);
        void schedule(CompilerEvent ev);
        void schedule(IEnumerable<CompilerEvent> evs);
        void schedule(string pass, CompilerEvent ev);
        void schedule(string pass, IEnumerable<CompilerEvent> ev);
    }


    //expected events
    public class LexicalMatchEvent<TToken, TNode> : CompilerEvent
    {
        public LexicalMatchEvent(IEnumerable<ILexicalMatch<TToken, TNode>> matchers) :
            base(CompilerStage.Lexical, "")
        {
            Matchers = matchers;
        }

        public IEnumerable<ILexicalMatch<TToken, TNode>> Matchers { get; set; }
    };

    public class LexicalExtensionEvent<TToken, TNode> : CompilerEvent
    {
        public LexicalExtensionEvent(LexicalExtension<TToken> extension, int lexicalId, Func<ISyntacticalMatchResult<TNode>, LexicalExtension<TToken>, TNode> handler) :
            base(CompilerStage.Lexical, "")
        {
            Extension = extension;
            LexicalId = lexicalId;
            Handler = handler;
        }

        public LexicalExtension<TToken> Extension { get;  }
        public int LexicalId { get; }
        public Func<ISyntacticalMatchResult<TNode>, LexicalExtension<TToken>, TNode> Handler { get; }
    }


    public class SyntacticalMatchEvent<TNode> : CompilerEvent
    {
        public SyntacticalMatchEvent(IEnumerable<ISyntacticalMatch<TNode>> matchers) :
            base(CompilerStage.Syntactical, "")
        {
            Matchers = matchers;
        }

        public IEnumerable<ISyntacticalMatch<TNode>> Matchers { get; set; }
    };

    public class SyntacticalNodeEvent<TNode> : CompilerEvent
    {
        public SyntacticalNodeEvent(int node, Func<TNode, TNode> handler, string pass) :
            base(CompilerStage.Syntactical, pass)
        {
            Node = node;
            Handler = handler;
        }

        public int Node { get; }
        public Func<TNode, TNode> Handler { get; }
    };
}
