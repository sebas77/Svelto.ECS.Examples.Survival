///
/// Unit tests to write:
/// Restart a task with compiled generated IEnumerator
/// Restart a task with IEnumerator class
/// Restart a task after SetEnumerator has been called (this must be still coded, as it must reset some values)
/// Restart a task just restarted (pendingRestart == true)
/// Staggered Runner example
/// 
/// 

using System;
using System.Collections;
using System.Threading;

namespace Svelto.Tasks
{
    public class EnumeratorWrapper : IEnumerator
    {
        public bool MoveNext()
        {
            MemoryBarrier();
            return _completed == false;
        }

        public void Reset()
        {
            _completed = false;
            MemoryBarrier();
        }

        void MemoryBarrier()
        {
#if NETFX_CORE
            Interlocked.MemoryBarrier();
#else
            Thread.MemoryBarrier();
#endif
        }

        public object Current { get; private set; }

        internal void Completed()
        {
            _completed = true;
            MemoryBarrier();
        }

        volatile bool _completed;
    }

}
