using System;
using System.Collections.Generic;
using System.Collections;
using Svelto.DataStructures;
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
#endif

namespace Svelto.Tasks
{
	public class MultiThreadParallelTaskCollection: TaskCollection
    {
        public event Action		onComplete;
		
		override public 	float progress { get { return _progress;} }
 
		public MultiThreadParallelTaskCollection(MultiThreadRunner runner):base()
        {
			_maxConcurrentTasks = uint.MaxValue;
			_runner = runner;
		}

		public MultiThreadParallelTaskCollection(MultiThreadRunner runner, uint maxConcurrentTasks):base()
		{
			_maxConcurrentTasks = maxConcurrentTasks;
			_runner = runner;
		}
		
		override public IEnumerator GetEnumerator()
		{
			_totalTasks = _tasksToExecute = (uint)registeredEnumerators.Count;
			_tasksLaunched = 0;

			if (_tasksToExecute > 0)
			{
				isRunning = true;
				
				RunMultiThreadParallelTasks();

				uint tasksToExecute;

				do 
                {
                    DateTime time = DateTime.Now;

                    if ((DateTime.Now - time).Milliseconds < 100)
                        yield return null;
                    
					lock (_locker) tasksToExecute = _tasksToExecute;
				} 
                while (tasksToExecute > 0);
				
				isRunning = false;
			}
						
			if (onComplete != null)
				onComplete();
        }

		IEnumerator ParallelTask(IEnumerator enumerator)
		{
			Stack<IEnumerator> stack = new Stack<IEnumerator>();
			stack.Push(enumerator);

			lock (_locker) _tasksLaunched++;

			while (stack.Count > 0) 
			{
				IEnumerator ce = stack.Peek();
				//without popping it.
				if (ce.MoveNext () == false) 
				{
					lock (_locker) _progress = (float)(_totalTasks - _registeredEnumerators.Count) / _totalTasks;

					stack.Pop();

					if (_registeredEnumerators.Count > 0)
						RunNewParallelTask();

					lock (_locker) _tasksToExecute--;
				}
				else
				{
					//ok the iteration is not over
					if (ce.Current != null && ce.Current != ce) 
					{
						if (ce.Current is IEnumerable)
							//what we got from the enumeration is an IEnumerable?
							stack.Push (((IEnumerable)ce.Current).GetEnumerator ());
						else
						if (ce.Current is IEnumerator)
							//what we got from the enumeration is an IEnumerator?
							stack.Push(ce.Current as IEnumerator);
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
						else
						if (ce.Current is WWW || ce.Current is YieldInstruction)
							throw new Exception ("Unity YieldInstructions cannot run in other threads");
#endif
					}
				}

				yield return null; //in order to be able to pause this task, the yield must be here
			}
		}

		void RunMultiThreadParallelTasks()
		{
			if (_maxConcurrentTasks == uint.MaxValue)
                _maxConcurrentTasks = (uint)Environment.ProcessorCount;
			else
				_maxConcurrentTasks = Math.Min(_maxConcurrentTasks, (uint)(registeredEnumerators.Count));

			_maxConcurrentTasks = Math.Min(MAX_CONCURRENT_TASK, _maxConcurrentTasks);

			_registeredEnumerators = new ThreadSafeQueue<IEnumerator>(registeredEnumerators);

			for (int i = 0; i < _maxConcurrentTasks; i++)
				if (_registeredEnumerators.Count > 0)
					RunNewParallelTask();
		}

		void RunNewParallelTask()
		{
			_runner.StartCoroutine(ParallelTask(_registeredEnumerators.Dequeue()));
		}
		
		volatile float 					_progress;
		volatile float					_totalTasks;

		uint							_tasksToExecute;
		uint							_tasksLaunched;
		uint					 		_maxConcurrentTasks;
		ThreadSafeQueue<IEnumerator> 	_registeredEnumerators;
		MultiThreadRunner				_runner;
		object							_locker = new object();

		const int						MAX_CONCURRENT_TASK = 8;
    }
}
