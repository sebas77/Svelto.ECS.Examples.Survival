using System.Collections;
using System.Threading;

namespace Svelto.Tasks
{
	public class MultiThreadRunner: IRunner
    {
		public MultiThreadRunner()
		{
			paused = false;
			stopped = false;
		}

		public void StartCoroutine(IEnumerator task)
        {
			stopped = false;
			paused = false;

            ThreadPool.QueueUserWorkItem((stateInfo) => { while (stopped == false && task.MoveNext() == true) { } });
        }

        public void StopAllCoroutines()
        {
            StopManagedCoroutines();
        }

        public void StopManagedCoroutines()
        {
            stopped = true;
            Thread.MemoryBarrier();
        }

		public bool paused { set; get; }
		public bool stopped { private set; get; }
    }
}
