﻿using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Excess.Concurrent.Runtime
{
    public class ConcurrentObject : IConcurrentObject
    {
        IConcurrentApp _app; 
        public void __start(IConcurrentApp app)
        {
            if (_app != null)
                throw new InvalidOperationException("already running");

            _app = app;
            __started();
        }

        protected IConcurrentApp App { get { return _app; } }

        protected virtual void __started()
        {
        }

        protected T spawn<T>(params object[] args) where T : ConcurrentObject
        {
            return _app.Spawn<T>(args);
        }

        int _busy = 0;
        public virtual void __enter(Action what, Action<Exception> failure)
        {
            if (_app == null)
                throw new InvalidOperationException("not running");

            var was_busy = Interlocked.CompareExchange(ref _busy, 1, 0) == 1;
            if (was_busy)
            {
                _app.Schedule(this, what, failure);
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

        public void __advance(IEnumerator<Expression> signal)
        {
            if (!signal.MoveNext())
                return;

            var expr = signal.Current;
            expr.Continuator = signal;
            if (expr.Start != null)
                expr.Start(expr);
        }

        Dictionary<string, List<Action>> _listeners = new Dictionary<string, List<Action>>();
        public void __listen(string signal, Action callback)
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

        protected double rand() => _app.rand();
        protected double rand(double from, double to) => from + _app.rand()*(to - from);
        protected int rand(int from, int to) => from + (int)_app.rand() * (to - from);
    }
}
