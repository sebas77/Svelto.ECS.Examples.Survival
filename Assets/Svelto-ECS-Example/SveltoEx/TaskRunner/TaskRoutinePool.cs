using System.Collections;
using System.Collections.Generic;

namespace Svelto.Tasks.Internal
{
    internal class TaskRoutinePool
    {
        internal TaskRoutinePool(IRunner runner)
        {
            _runner = runner;
        }

        internal TaskRoutine RetrieveTask()
        {
            if (_pool.Count > 0)
                return _pool.Pop();

            return CreateEmptyTask();
        }

        internal void Start(IEnumerator enumerator, bool isSimple)
        {
            TaskRoutine task = RetrieveTask();

            task.Start(EnumeratorDecorator(enumerator, task), isSimple);
        }

        IEnumerator EnumeratorDecorator(IEnumerator enumerator, TaskRoutine task)
        {
            while (enumerator.MoveNext() == true)
                yield return enumerator.Current;

            _pool.Push(task);
        }

        TaskRoutine CreateEmptyTask()
	    {
		    PausableTask ptask = new PausableTask(_runner);

		    return new TaskRoutine(ptask);
	    }

        Stack<TaskRoutine>  _pool = new Stack<TaskRoutine>();
        IRunner             _runner;
    }
}
