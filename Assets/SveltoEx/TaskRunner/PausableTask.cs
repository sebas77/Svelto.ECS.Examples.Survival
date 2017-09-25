///
/// Unit tests to write:
/// Restart a task with compiled generated IEnumerator
/// Restart a task with IEnumerator class
/// Restart a task after SetEnumerator has been called (this must be still coded, as it must reset some values)
/// Restart a task just restarted (pendingRestart == true)
/// Staggered Runner example
/// 
/// 

using Svelto.Tasks.Internal;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
#if NETFX_CORE
using System.Reflection;
#endif

namespace Svelto.Tasks
{
    public class PausableTaskException : Exception
    {
        public PausableTaskException(Exception e)
            : base(e.ToString(), e)
        { }
    }
}

namespace Svelto.Tasks
{
    public partial class PausableTask : ITaskRoutine, IEnumerator
    {
        public object Current
        {
            get
            {
                if (_enumerator != null)
                    return _enumerator.Current;

                return null;
            }
        }

        public ITaskRoutine SetScheduler(IRunner runner)
        {
            _runner = runner;

            return this;
        }

        public ITaskRoutine SetEnumeratorProvider(Func<IEnumerator> taskGenerator)
        {
            _taskEnumerator = null;
            _taskGenerator = taskGenerator;

            return this;
        }

        public ITaskRoutine SetEnumerator(IEnumerator taskEnumerator)
        {
            _taskGenerator = null;
            _taskEnumerator = taskEnumerator;
//#if UNITY_EDITOR
  //          _compilerGenerated = IsCompilerGenerated(taskEnumerator.GetType());
//#else
            _compilerGenerated = false;
//#endif

            return this;
        }

        public override string ToString()
        {
            if (_taskGenerator == null && _taskEnumerator == null)
                return base.ToString();

            if (_taskEnumerator != null)
                return _taskEnumerator.ToString();
            else
#if NETFX_CORE
                return _taskGenerator.GetMethodInfo().DeclaringType + "." + _taskGenerator.GetMethodInfo().Name;
#else
                return _taskGenerator.Method.ReflectedType + "." + _taskGenerator.Method.Name;
#endif
        }

        public bool MoveNext()
        {
            if (_stopped == true || _runner.stopped == true)
            {
                _completed = true;

                //this is needed to avoid to create multiple CoRoutine when
                //Stop and Start are called in the same frame
                if (_pendingRestart == true)
                {
                    _pendingRestart = false;
                    //start new coroutine using this task
                    Restart(_pendingEnumerator);

                    return false;
                }

                if (_onStop != null)
                    _onStop();
            }
            else
            if (_runner.paused == false && _paused == false)
            {
                try
                {
                    _completed = !_enumerator.MoveNext();

                    if (_enumerator.Current == Break.It)
                    {
                        Stop();

                        _completed = true;

                        if (_onStop != null)
                            _onStop();
                    }
                }
                catch (Exception e)
                {
                    _completed = true;

                    if (_onFail != null && (e is TaskYieldsIEnumerableException) == false)
                        _onFail(new PausableTaskException(e));
                    else
                    {
                        if (_pool != null)
                            _pool.PushTaskBack(this);

                        throw new PausableTaskException(e);
                    }
                }
            }

            if (_completed == true)
            {
                _enumeratorWrap.Completed();
                if (_pool != null)
                    _pool.PushTaskBack(this);
            }

            return !_completed;
        }

        //Reset task on reuse, when fetched from the Pool
        public void Reset()
        {
            _enumeratorWrap.Reset();

            _pendingEnumerator = null;
            _taskGenerator     = null;
            _taskEnumerator    = null;
            _runner            = null;
            _onFail            = null;
            _onStop            = null;

            _stopped           = false;
            _paused            = false;
            _threadSafe        = false;
            _compilerGenerated = false;
            _completed         = false;
            _started           = false;
            _pendingRestart    = false;
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            if (_started == false)
                Start();

            _paused = false;
        }

        public EnumeratorWrapper Start(Action<PausableTaskException> onFail = null, Action onStop = null)
        {
            _threadSafe = false;

            _onStop = onStop;
            _onFail = onFail;

            InternalStart();

            return _enumeratorWrap;
        }

        public EnumeratorWrapper ThreadSafeStart(Action<PausableTaskException> onFail = null, Action onStop = null)
        {
            _threadSafe = true;

            _onStop = onStop;
            _onFail = onFail;

            InternalStart();

            return _enumeratorWrap;
        }

        public void Stop()
        {
            //pay attention, completed cannot be put to true here, because it the task restarts
            //it must ends naturally through the fact that _stopped is true
            _stopped = true;
            _started = false;
        }

        internal PausableTask(IPausableTaskPool pool) : this()
        {
            _pool = pool;
        }

        internal PausableTask()
        {
            Stop();

            _enumeratorWrap = new EnumeratorWrapper();
            _enumerator = new CoroutineEx();
        }

        bool IsCompilerGenerated(Type t)
        {
#if NETFX_CORE
            var attr = t.GetTypeInfo().GetCustomAttribute(typeof(CompilerGeneratedAttribute));
#else
            var attr = Attribute.GetCustomAttribute(t, typeof(CompilerGeneratedAttribute));
#endif

            return attr != null;
        }

        /// <summary>
        /// A Pausable Task cannot be recycled from the pool if hasn't been
        /// previously completed. The Pending logic is valid for normal
        /// tasks that are held and reused by other classes.
        /// </summary>
        /// <param name="task"></param>
        void InternalStart()
        {
            if (_taskGenerator == null && _taskEnumerator == null)
                throw new Exception("An enumerator or enumerator provider is required to enable this function, please use SetEnumeratorProvider/SetEnumerator before to call start");

            IEnumerator enumerator = _taskEnumerator ?? _taskGenerator();

            if (_pendingRestart == true)
                throw new Exception("Cannot restart a task that is already pending a restart");

            if (_started == true && _completed == false)
            {
                Stop(); //if it's reused, must stop naturally
                _pendingEnumerator = enumerator;
                _pendingRestart = true;
            }
            else
                Restart(enumerator);

            Resume(); //if it's paused, must resume
        }

        void Restart(IEnumerator task)
        {
            if (_taskEnumerator != null && _completed == true)
            {
                if (_compilerGenerated == false)
                    task.Reset();
                else
                    throw new Exception(
                        "Cannot restart an IEnumerator without a valid Reset function, use SetEnumeratorProvider instead");
            }

            if (_runner == null)
                throw new Exception("SetScheduler function has never been called");

            _stopped = false;
            _completed = false;
            _started = true;
            _pendingEnumerator = null;
            _pendingRestart = false;

            SetTask(task);

            if (_threadSafe == false)
                _runner.StartCoroutine(this);
            else
                _runner.StartCoroutineThreadSafe(this);
        }

        void SetTask(IEnumerator task)
        {
            if ((task is CoroutineEx) == false)
                _enumerator.Reuse(task);
            else
                _enumerator = task as CoroutineEx;
        }

        IRunner _runner;
        CoroutineEx _enumerator;

        bool _stopped;
        bool _paused;
        bool _threadSafe;
        bool _compilerGenerated;
        bool _pendingRestart;

        IEnumerator _pendingEnumerator;
        IEnumerator _taskEnumerator;
        EnumeratorWrapper _enumeratorWrap;

        IPausableTaskPool _pool;
        Func<IEnumerator> _taskGenerator;

        Action<PausableTaskException> _onFail;
        Action _onStop;

        volatile bool _completed = false;
        volatile bool _started = false;
    }
}
