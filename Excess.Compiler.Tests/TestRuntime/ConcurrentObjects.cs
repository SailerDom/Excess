﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excess.Compiler.Tests.TestRuntime
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Threading;
    using Spawner = Func<object[], ConcurrentObject>;

    public class Node
    {
        IDictionary<string, Spawner> _types;
        public Node(int threads, IDictionary<string, Spawner> types)
        {
            _types = types;

            Debug.Assert(threads > 0);
            createThreads(threads);
        }

        public T Spawn<T>(params object[] args) where T : ConcurrentObject, new()
        {
            var result = new T();
            result.startRunning(this, args);
            return result;
        }

        public ConcurrentObject Spawn(string type, params object[] args)
        {
            var caller = null as Func<object[], ConcurrentObject>;
            if (!_types.TryGetValue(type, out caller))
                throw new InvalidOperationException(type + " is not defined");

            var result = caller(args); 
            result.startRunning(this, args);
            return result;
        }

        public void Start(ConcurrentObject @object, params object[] args)
        {
            @object.startRunning(this, args);
        }

        private class Event
        {
            public int Tries { get; set; }
            public ConcurrentObject Target { get; set; }
            public Action What { get; set; }
            public Action<Exception> Failure { get; set; }
        }

        ConcurrentQueue<Event> _queue = new ConcurrentQueue<Event>();

        public void Queue(ConcurrentObject who, Action what, Action<Exception> failure)
        {
            _queue.Enqueue(new Event
            {
                Tries = 0,
                Target = who,
                What = what,
                Failure = failure
            });
        }

        CancellationTokenSource _stop = new CancellationTokenSource();
        public void Stop()
        {
            _stop.Cancel();
            Thread.Sleep(1);
        }

        private void createThreads(int threads)
        {
            var cancellation = _stop.Token;
            for (int i = 0; i < threads; i++)
            {
                var thread = new Thread(() => {
                    while (true)
                    {

                        Event message;
                        _queue.TryDequeue(out message);
                        if (cancellation.IsCancellationRequested)
                            break;

                        if (message == null)
                        {
                            Thread.Sleep(1);
                            continue;
                        }

                        message.Target.__enter(message.What, message.Failure);
                    }
                });

                thread.Priority = ThreadPriority.AboveNormal;
                thread.Start();
            }
        }

        public void waitForCompletion()
        {
            _stop.Token.WaitHandle.WaitOne();
        }
    }

    public class ConcurrentObject
    {
        protected Node _node;
        internal void startRunning(Node node, object[] args)
        {
            Debug.Assert(_busy == 0);
            _node = node;
            __start(args);
        }

        protected Node Node { get { return _node; } }
        protected virtual void __start(object[] args)
        {
        }

        protected T spawn<T>(params object[] args) where T : ConcurrentObject, new()
        {
            return _node.Spawn<T>(args);
        }

        int _busy = 0;
        protected internal void __enter(Action what, Action<Exception> failure)
        {
            var was_busy = Interlocked.CompareExchange(ref _busy, 1, 0) == 1;
            if (was_busy)
            {
                _node.Queue(this, what, failure);
            }
            else
            {
                try
                {
                    what();
                }
                catch (Exception ex)
                {
                    if (failure != null)
                        failure(ex); 
                    else
                        throw;
                }
                finally
                {
                    Interlocked.CompareExchange(ref _busy, 0, 1);
                }
            }
        }

        protected void __advance(IEnumerator<Expression> thread)
        {
            if (!thread.MoveNext())
                return;

            var expr = thread.Current;
            expr.Continuator = thread;
            if (expr.Start != null)
                expr.Start(expr);
        }

        Dictionary<string, List<Action>> _listeners = new Dictionary<string, List<Action>>();
        protected void __listen(string signal, Action callback)
        {
            List<Action> actions;
            if (!_listeners.TryGetValue(signal, out actions))
            {
                actions = new List<Action>();
                _listeners[signal] = actions;
            }

            actions.Add(callback);
        }

        protected void __dispatch(string signal)
        {
            List<Action> actions;
            if (_listeners.TryGetValue(signal, out actions))
            {
                foreach (var action in actions)
                {
                    try
                    {
                        action();
                    }
                    catch
                    {
                    }
                }

                actions.Clear();
            }
        }

        protected bool __awaiting(string signal)
        {
            return _listeners
                .Where(kvp => kvp.Key == signal
                           && kvp.Value.Any())
                .Any();

        }

        Random __rand = new Random(); //td: test only
        protected double rand()
        {
            return __rand.NextDouble();
        }

        protected double rand(double from, double to)
        {
            return from + (to - from)*__rand.NextDouble();
        }
    }

    public class Expression
    {
        public Action<Expression> Start { get; set; }
        public IEnumerator<Expression> Continuator { get; set; }
        public Action<Expression> End { get; set; }
        public Exception Failure { get; set; }

        List<Exception> _exceptions = new List<Exception>();
        protected void __complete(bool success, Exception failure)
        {
            Debug.Assert(Continuator != null);
            Debug.Assert(End != null);

            IEnumerable<Exception> allFailures = _exceptions;
            if (failure != null)
                allFailures = allFailures.Union(new[] { failure });

            if (!success)
            {
                Debug.Assert(allFailures.Any());

                if (allFailures.Count() == 1)
                    Failure = allFailures.First();
                else
                    Failure = new AggregateException(allFailures);
            }

            End(this);
        }

        protected bool tryUpdate(bool? v1, bool? v2, ref bool? s1, ref bool? s2, Exception ex)
        {
            Debug.Assert(!v1.HasValue || !v2.HasValue);

            if (ex != null)
            {
                Debug.Assert((v1.HasValue && !v1.Value) || (v2.HasValue && !v2.Value));
                _exceptions.Add(ex);
            }

            if (v1.HasValue)
            {
                if (s1.HasValue)
                    return false;

                s1 = v1;
            }
            else
            {
                Debug.Assert(v2.HasValue);
                if (s2.HasValue)
                    return false;

                s2 = v2;
            }

            return true;
        }
    }
}
