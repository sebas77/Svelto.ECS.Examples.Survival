using Svelto.Tasks.Internal;
using System;
using System.Collections;

namespace Svelto.Tasks
{
	public class TaskRoutine
	{
        internal TaskRoutine(PausableTask task, Func<IEnumerator> taskGenerator)
		{
            _task = task;
            _taskGenerator = taskGenerator;
		}

        internal TaskRoutine(PausableTask task)
		{
            _task = task;
		}

		public void Start(bool isSimple)
		{
            _task.Start(_taskGenerator(), isSimple);
		}

        public void Start(IEnumerator taskGenerator, bool isSimple)
		{
            _task.Start(taskGenerator, isSimple);
		}

		public void Stop()
		{
			_task.Stop();
		}

		public void Pause()
		{
			_task.Pause();
		}

		public void Resume()
		{
			_task.Resume();
		}

		PausableTask 	  _task;
        Func<IEnumerator> _taskGenerator;
    }
}

