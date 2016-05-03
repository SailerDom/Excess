using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Excess.Concurrent.Runtime;

namespace Santa
{
    [Concurrent(id = "8fb03663-8c39-4709-83c4-4fbe15fd5b16")]
    [ConcurrentSingleton(id: "5bbefbfe-2ebb-49b1-bad8-ed906938070b")]
    public class __app : ConcurrentObject
    {
        protected override void __started()
        {
            var __enum = __concurrentmain(default (CancellationToken), null, null);
            __enter(() => __advance(__enum.GetEnumerator()), null);
        }

        private IEnumerable<Expression> __concurrentmain(CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)
        {
            var reindeers = new[]{"Dasher", "Dancer", "Prancer", "Vixen", "Comet", "Cupid", "Dunder", "Rudolph"};
            var elves = new[]{"Alabaster", "Bushy", "Pepper", "Shinny", "Sugarplum", "Wunorse", "Buddy", "Kringle", "Tinsel", "Jangle"};
            var santa = spawn<SantaClaus>();
            foreach (var reindeer in reindeers)
                spawn<Reindeer>(reindeer, santa);
            foreach (var elf in elves)
                spawn<Elf>(elf, santa);
            {
                __dispatch("main");
                if (__success != null)
                    __success(null);
                yield break;
            }
        }

        private static __app __singleton;
        public static void Start(IConcurrentApp app)
        {
            __singleton = app.Spawn<__app>();
        }

        public readonly Guid __ID = Guid.NewGuid();
        public static void Start(string[] args)
        {
            Arguments = args;
            var app = new ThreadedConcurrentApp(threadCount: 4, blockUntilNextEvent: true, priority: ThreadPriority.Normal);
            app.Start();
            app.Spawn(new __app());
            Await = () => app.AwaitCompletion();
            Stop = () => app.Stop();
        }

        public static string[] Arguments
        {
            get;
            set;
        }

        public static Action Stop
        {
            get;
            private set;
        }

        public static Action Await
        {
            get;
            private set;
        }
    }

    [Concurrent(id = "9e183a5e-852a-4900-9eaa-93556f471be4")]
    class SantaClaus : ConcurrentObject
    {
        List<Reindeer> _reindeer = new List<Reindeer>();
        List<Elf> _elves = new List<Elf>();
        bool _busy = false;
        [Concurrent]
        //a reindeer is ready for work
        public void reindeer(Reindeer r)
        {
            reindeer(r, default (CancellationToken), null, null);
        }

        [Concurrent]
        //and elf wants to meet with Santa
        public void elf(Elf e)
        {
            elf(e, default (CancellationToken), null, null);
        }

        private void cancelMeeting()
        {
            __dispatch("cancelMeeting");
        }

        private void meetingCanceled()
        {
            __dispatch("meetingCanceled");
        }

        private bool readyToDeliver()
        {
            return _reindeer.Count == 8;
        }

        private IEnumerable<Expression> __concurrentreindeer(Reindeer r, CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)
        {
            _reindeer.Add(r);
            if (readyToDeliver())
            {
                //in case we're meeting with elves
                if (_busy)
                {
                    var __expr5_var = new __expr5{Start = (___expr) =>
                    {
                        var __expr = (__expr5)___expr;
                        __advance((__concurrentcancelMeeting(__cancellation, (__res) =>
                        {
                            __expr.__op9(true, null, null);
                        }

                        , (__ex) =>
                        {
                            __expr.__op9(false, null, __ex);
                        }

                        )).GetEnumerator());
                    }

                    , End = (__expr) =>
                    {
                        __enter(() => __advance(__expr.Continuator), __failure);
                    }

                    , __start5 = (___expr) =>
                    {
                        var __expr = (__expr5)___expr;
                        __enter(() =>
                        {
                            __listen("meetingCanceled", () =>
                            {
                                __expr.__op9(null, true, null);
                            }

                            );
                        }

                        , __failure);
                    }
                    };
                    yield return __expr5_var;
                    if (__expr5_var.Failure != null)
                        throw __expr5_var.Failure;
                }

                //christmas!
                _busy = true;
                Console.WriteLine("Santa: Off to deliver toys!");
                {
                    var __expr6_var = new __expr6{Start = (___expr) =>
                    {
                        var __expr = (__expr6)___expr;
                        Task.Delay((int)((rand(5, 10)) * 1000)).ContinueWith(__task =>
                        {
                            __enter(() => __expr.__op10(true, null, null), (__ex) => __expr.__op10(false, null, __ex));
                        }

                        );
                        __expr.__op10(null, false, null);
                    }

                    , End = (__expr) =>
                    {
                        __enter(() => __advance(__expr.Continuator), __failure);
                    }
                    };
                    yield return __expr6_var;
                    if (__expr6_var.Failure != null)
                        throw __expr6_var.Failure;
                }

                Console.WriteLine("Santa: Merry Christmas, enjoy the toys!");
                //is over 
                foreach (var rd in _reindeer)
                {
                    rd.unharness();
                }

                _reindeer.Clear();
                _busy = false;
                App.Stop();
            }

            {
                __dispatch("reindeer");
                if (__success != null)
                    __success(null);
                yield break;
            }
        }

        public Task<object> reindeer(Reindeer r, CancellationToken cancellation)
        {
            var completion = new TaskCompletionSource<object>();
            Action<object> __success = (__res) => completion.SetResult((object)__res);
            Action<Exception> __failure = (__ex) => completion.SetException(__ex);
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentreindeer(r, __cancellation, __success, __failure).GetEnumerator()), __failure);
            return completion.Task;
        }

        public void reindeer(Reindeer r, CancellationToken cancellation, Action<object> success, Action<Exception> failure)
        {
            var __success = success;
            var __failure = failure;
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentreindeer(r, __cancellation, __success, __failure).GetEnumerator()), failure);
        }

        private IEnumerable<Expression> __concurrentelf(Elf e, CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)
        {
            if (_busy)
            {
                e.advice(false);
                {
                    __dispatch("elf");
                    if (__success != null)
                        __success(null);
                    yield break;
                }
            }

            _elves.Add(e);
            if (_elves.Count == 3)
            {
                _busy = true;
                Console.WriteLine("Santa: hey guys, need help?");
                var __expr7_var = new __expr7{Start = (___expr) =>
                {
                    var __expr = (__expr7)___expr;
                    Task.Delay((int)((1) * 1000)).ContinueWith(__task =>
                    {
                        __enter(() => __expr.__op11(true, null, null), (__ex) => __expr.__op11(false, null, __ex));
                    }

                    );
                    __listen("cancelMeeting", () =>
                    {
                        __expr.__op11(null, true, null);
                    }

                    );
                }

                , End = (__expr) =>
                {
                    __enter(() => __advance(__expr.Continuator), __failure);
                }
                };
                yield return __expr7_var;
                if (__expr7_var.Failure != null)
                    throw __expr7_var.Failure;
                var isDelivering = readyToDeliver();
                if (isDelivering) //therefore the meeting was canceled
                {
                    Console.WriteLine("Santa: sorry fellows, got toys to deliver!");
                    meetingCanceled();
                }
                else
                {
                    Console.WriteLine("Santa: Good meeting, little fellas!");
                    _busy = false;
                }

                //adjourned
                foreach (var elf in _elves)
                {
                    elf.advice(!isDelivering);
                }

                _elves.Clear();
            }

            {
                __dispatch("elf");
                if (__success != null)
                    __success(null);
                yield break;
            }
        }

        public Task<object> elf(Elf e, CancellationToken cancellation)
        {
            var completion = new TaskCompletionSource<object>();
            Action<object> __success = (__res) => completion.SetResult((object)__res);
            Action<Exception> __failure = (__ex) => completion.SetException(__ex);
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentelf(e, __cancellation, __success, __failure).GetEnumerator()), __failure);
            return completion.Task;
        }

        public void elf(Elf e, CancellationToken cancellation, Action<object> success, Action<Exception> failure)
        {
            var __success = success;
            var __failure = failure;
            var __cancellation = cancellation;
            __enter(() => __advance(__concurrentelf(e, __cancellation, __success, __failure).GetEnumerator()), failure);
        }

        private IEnumerable<Expression> __concurrentcancelMeeting(CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)
        {
            if (false && !__awaiting("cancelMeeting"))
                throw new InvalidOperationException("cancelMeeting" + " can not be executed in this state");
            __dispatch("cancelMeeting");
            if (__success != null)
                __success(null);
            yield break;
        }

        private IEnumerable<Expression> __concurrentmeetingCanceled(CancellationToken __cancellation, Action<object> __success, Action<Exception> __failure)
        {
            if (false && !__awaiting("meetingCanceled"))
                throw new InvalidOperationException("meetingCanceled" + " can not be executed in this state");
            __dispatch("meetingCanceled");
            if (__success != null)
                __success(null);
            yield break;
        }

        private class __expr5 : Expression
        {
            public void __op9(bool ? v1, bool ? v2, Exception __ex)
            {
                if (!tryUpdate(v1, v2, ref __op9_Left, ref __op9_Right, __ex))
                    return;
                if (v1.HasValue)
                {
                    if (__op9_Left.Value)
                        __start5(this);
                    else
                        __complete(false, __ex);
                }
                else
                {
                    if (__op9_Right.Value)
                        __complete(true, null);
                    else
                        __complete(false, __ex);
                }
            }

            private bool ? __op9_Left;
            private bool ? __op9_Right;
            public Action<__expr5> __start5;
        }

        private class __expr6 : Expression
        {
            public void __op10(bool ? v1, bool ? v2, Exception __ex)
            {
                if (!tryUpdate(v1, v2, ref __op10_Left, ref __op10_Right, __ex))
                    return;
                if (v1.HasValue)
                {
                    if (__op10_Left.Value)
                        __complete(true, null);
                    else if (__op10_Right.HasValue)
                        __complete(false, __ex);
                }
                else
                {
                    if (__op10_Right.Value)
                        __complete(true, null);
                    else if (__op10_Left.HasValue)
                        __complete(false, __ex);
                }
            }

            private bool ? __op10_Left;
            private bool ? __op10_Right;
        }

        private class __expr7 : Expression
        {
            public void __op11(bool ? v1, bool ? v2, Exception __ex)
            {
                if (!tryUpdate(v1, v2, ref __op11_Left, ref __op11_Right, __ex))
                    return;
                if (v1.HasValue)
                {
                    if (__op11_Left.Value)
                        __complete(true, null);
                    else if (__op11_Right.HasValue)
                        __complete(false, __ex);
                }
                else
                {
                    if (__op11_Right.Value)
                        __complete(true, null);
                    else if (__op11_Left.HasValue)
                        __complete(false, __ex);
                }
            }

            private bool ? __op11_Left;
            private bool ? __op11_Right;
        }

        public readonly Guid __ID = Guid.NewGuid();
    }

    class Program
    {
        static void Main(string[] args)
        {
            __app.Start(args);
            {
            }

            __app.Await();
        }
    }
}