//#define old

#if old
using System;
using System.Collections;
using System.Threading;
using Svelto.DataStructures;

#if NETFX_CORE
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;
#endif

namespace Svelto.Tasks
{
    //The multithread runner always uses just one thread to run all the couroutines
    //If you want to use a separate thread, you will need to create another MultiThreadRunner
    public class MultiThreadRunner : IRunner
    {
        public MultiThreadRunner()
        {
            paused = false;
        }

        public void StartCoroutineThreadSafe(IPausableTask task)
        {
            StartCoroutine(task);
        }

        public void StartCoroutine(IPausableTask task)
        {
            paused = false;

            _newTaskRoutines.Enqueue(task);

            MemoryBarrier();
            if (_isAlive == false)
            {
                _waitForflush = false;
                _isAlive = true;
                MemoryBarrier();

#if NETFX_CORE
                IAsyncAction asyncAction = ThreadPool.RunAsync
                    ((workItem) =>
                    {
                        _threadName = System.Threading.Thread.CurrentThread.Name;

                        RunCoroutineFiber();
                    });

#else
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback);
#endif
            }
        }

        void ThreadPoolCallback(object stateInfo)
        {
            _threadID = Thread.CurrentThread.ManagedThreadId;

            RunCoroutineFiber();
        }

        public void StopAllCoroutines()
        {
            _newTaskRoutines.Clear();
            stopped = true;
            _waitForflush = true;
            MemoryBarrier();
        }

        public bool paused
        {
            set
            {
                _paused = value;
                MemoryBarrier();
            }
            get
            {
                MemoryBarrier();
                return _paused;
            }
        }

        public bool stopped
        {
            get; private set;
        }

        public int numberOfRunningTasks
        {
            get { return _coroutines.Count; }
        }

        void RunCoroutineFiber()
        {
            while (_coroutines.Count > 0 || _newTaskRoutines.Count > 0)
            {
                if (_waitForflush == false && _newTaskRoutines.Count > 0) 
                    _coroutines.AddRange(_newTaskRoutines.DequeueAll());

                for (var i = 0; i < _coroutines.Count; i++)
                {
                    var enumerator = _coroutines[i];

                    try
                    {
                        bool result;
#if TASKS_PROFILER_ENABLED
                        result = Profiler.TaskProfiler.MonitorUpdateDuration(enumerator, _threadID);
#else
                        result = enumerator.MoveNext();
#endif
                        if (result == false)
                        {
                            var disposable = enumerator as IDisposable;
                            if (disposable != null)
                                disposable.Dispose();

                            _coroutines.UnorderedRemoveAt(i--);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.InnerException != null)
                            Utility.Console.LogException(e.InnerException);
                        else
                            Utility.Console.LogException(e);

                        _coroutines.UnorderedRemoveAt(i--);
                    }
                }

                if (_waitForflush == true && _coroutines.Count == 0)
                {  //this process is more complex than I like, not 100% sure it covers all the cases yet
                    _waitForflush = false;
                    stopped = false;
                }
            }

            _isAlive = false;
            MemoryBarrier();
        }

        static public void MemoryBarrier()
        {
#if NETFX_CORE
            Interlocked.MemoryBarrier();
#else
            Thread.MemoryBarrier();
#endif
        }

        readonly FasterList<IEnumerator> _coroutines = new FasterList<IEnumerator>();

        volatile bool _isAlive;
        readonly ThreadSafeQueue<IEnumerator> _newTaskRoutines = new ThreadSafeQueue<IEnumerator>();

        bool          _paused;
        volatile int  _threadID;
        volatile bool _waitForflush;
    }
}
#else
using System;
using System.Threading;
using Svelto.DataStructures;
using Console = Utility.Console;

#if NETFX_CORE
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;
#endif

namespace Svelto.Tasks
{
    //The multithread runner always uses just one thread to run all the couroutines
    //If you want to use a separate thread, you will need to create another MultiThreadRunner
    public class MultiThreadRunner : IRunner
    {
        public override string ToString()
        {
            return _name;
        }

        public void StartCoroutineThreadSafe(IPausableTask task)
        {
            StartCoroutine(task);
        }

        public MultiThreadRunner()
        {
            _thread = new Thread(() =>
            {
                _threadID = Thread.CurrentThread.ManagedThreadId;
                _name = _threadID.ToString();

                RunCoroutineFiber();
            });

            _thread.IsBackground = true;
            _thread.Start();
        }

        public void StartCoroutine(IPausableTask task)
        {
            paused = false;

            _newTaskRoutines.Enqueue(task);

            MemoryBarrier();
            if (_isAlive == false)
            {
                _isAlive = true;

                lock (_locker) Monitor.Pulse(_locker);
            }
        }

        public void StopAllCoroutines()
        {
            _newTaskRoutines.Clear();

            _waitForflush = true;
            MemoryBarrier();
        }

        public bool paused
        {
            set
            {
                _paused = value;
                MemoryBarrier();
            }
            get
            {
                MemoryBarrier();
                return _paused;
            }
        }

        public bool stopped
        {
            get
            {
                MemoryBarrier();
                return _isAlive == false;
            }
        }

        public int numberOfRunningTasks
        {
            get { return _coroutines.Count; }
        }

        void RunCoroutineFiber()
        {
            while (true)
            {
                MemoryBarrier();

				if (_newTaskRoutines.Count > 0 && false == _waitForflush) //don't start anything while flushing
                    _coroutines.AddRange(_newTaskRoutines.DequeueAll());

                for (var i = 0; i < _coroutines.Count; i++)
                {
                    var enumerator = _coroutines[i];

                    try
                    {
#if TASKS_PROFILER_ENABLED
                        bool result = Profiler.TaskProfiler.MonitorUpdateDuration(enumerator, _threadID);
#else
                        bool result = enumerator.MoveNext();
#endif
                        if (result == false)
                        {
                            var disposable = enumerator as IDisposable;
                            if (disposable != null)
                                disposable.Dispose();

                            _coroutines.UnorderedRemoveAt(i--);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.InnerException != null)
                            Console.LogException(e.InnerException);
                        else
                            Console.LogException(e);

                        _coroutines.UnorderedRemoveAt(i--);
                    }
                }

                while (_coroutines.Count == 0 && _newTaskRoutines.Count == 0)
                {
                    _isAlive = false;
                    _waitForflush = false;
                    lock (_locker) Monitor.Wait(_locker);
                }
            }
        }

        public static void MemoryBarrier()
        {
#if NETFX_CORE
            Interlocked.MemoryBarrier();
#else
            Thread.MemoryBarrier();
#endif
        }

        readonly FasterList<IPausableTask> _coroutines = new FasterList<IPausableTask>();
        readonly ThreadSafeQueue<IPausableTask> _newTaskRoutines = new ThreadSafeQueue<IPausableTask>();

        Thread _thread;
        string _name;

        volatile bool _paused;
        volatile bool _isAlive;
        volatile int  _threadID;
        volatile bool _waitForflush;

        object _locker = new object();
    }
}
#endif