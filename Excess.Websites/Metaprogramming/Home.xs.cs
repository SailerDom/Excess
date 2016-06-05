﻿#line 1 "C:\dev\Excess\Excess.Websites\metaprogramming\Home.xs"
using System;
#line hidden

using System.Collections.Generic;
using System.Linq;
using Middleware;
using System.Threading;
using System.Threading.Tasks;
using Excess.Concurrent.Runtime;


#line 4
namespace metaprogramming

#line 5
{
#line hidden

    [Service(id: "509ade34-c3b9-492e-b584-a87420429ab7")]
    [Concurrent(id = "707f3a16-67d7-4153-8216-8b78fb75a35a")]

#line 6
    public class Home : ConcurrentObject
#line hidden


#line 7
    {
#line hidden

        public 
#line 6
Home(
#line 10
ITranspiler ___transpiler, 
#line 11
IGraphTranspiler ___graphTranspiler)
#line hidden

        {

#line 10
            _transpiler = ___transpiler;
#line hidden


#line 11
            _graphTranspiler = ___graphTranspiler;
#line hidden

        }

        [Concurrent]

#line 14
        public string Transpile(string text)
#line hidden

        {
            return 
#line 14
Transpile(text, default (CancellationToken)).Result;
#line hidden

        }

        [Concurrent]

#line 20
        public string TranspileGraph(string text)
#line hidden

        {
            return 
#line 20
TranspileGraph(text, default (CancellationToken)).Result;
#line hidden

        }

        private IEnumerable<Expression> __concurrentTranspile
#line 14
(string text, CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)

#line 15
        {
#line hidden

            {
                __dispatch("Transpile");
                if (__success != null)
                    __success(
#line 17
_transpiler.Process(text));
#line hidden

                yield break;
            }

#line 18
        }

#line hidden

        public Task<
#line 14
string> Transpile(string text, CancellationToken cancellation)
#line hidden

        {
            var completion = new TaskCompletionSource<
#line 14
string>();
#line hidden

            Action<object> __success = (__res) => completion.SetResult((
#line 14
string)__res);
#line hidden

            Action<Exception> __failure = (__ex) => completion.SetException(__ex);
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentTranspile(
#line 14
text, __cancellation, __success, __failure).GetEnumerator()), __failure);
#line hidden

            return completion.Task;
        }

        public void Transpile(
#line 14
string text, CancellationToken cancellation, Action<object> success, Action<Exception> failure)
#line hidden

        {
            var __success = success;
            var __failure = failure;
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentTranspile(
#line 14
text, __cancellation, __success, __failure).GetEnumerator()), failure);
#line hidden

        }

        private IEnumerable<Expression> __concurrentTranspileGraph
#line 20
(string text, CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)

#line 21
        {
#line hidden

            {
                __dispatch("TranspileGraph");
                if (__success != null)
                    __success(
#line 22
_graphTranspiler.Process(text));
#line hidden

                yield break;
            }

#line 23
        }

#line hidden

        public Task<
#line 20
string> TranspileGraph(string text, CancellationToken cancellation)
#line hidden

        {
            var completion = new TaskCompletionSource<
#line 20
string>();
#line hidden

            Action<object> __success = (__res) => completion.SetResult((
#line 20
string)__res);
#line hidden

            Action<Exception> __failure = (__ex) => completion.SetException(__ex);
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentTranspileGraph(
#line 20
text, __cancellation, __success, __failure).GetEnumerator()), __failure);
#line hidden

            return completion.Task;
        }

        public void TranspileGraph(
#line 20
string text, CancellationToken cancellation, Action<object> success, Action<Exception> failure)
#line hidden

        {
            var __success = success;
            var __failure = failure;
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentTranspileGraph(
#line 20
text, __cancellation, __success, __failure).GetEnumerator()), failure);
#line hidden

        }

        public readonly Guid __ID = Guid.NewGuid();

#line 10
        ITranspiler _transpiler;

#line 11
        IGraphTranspiler _graphTranspiler;

#line 24
    }

#line 25
}