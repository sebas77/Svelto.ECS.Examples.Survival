using System;
using System.Collections;

/// <summary>
/// Single Task
///
/// - This is a more powerful version of passing a single IEnumerator as parameter of TaskRunner.Run
/// - The main difference is that a single task can execute other tasks returned by the single task
/// </summary>

namespace Svelto.Tasks.Internal
{
	internal class SingleTask: IEnumerator
	{
		public object Current 		{ get { return _enumerator.Current; } }
				 
		public SingleTask(IEnumerator enumerator)
		{
			if (enumerator is SingleTask || enumerator is PausableTask || enumerator is AsyncTask)
				throw new ArgumentException("Internal task used outside the framework scope");
			
			_task = new SerialTaskCollection();
			_task.Add(enumerator);
			
			_enumerator = _task.GetEnumerator();
						
			_onComplete = null;
		}
		
		public bool MoveNext()
		{
			if (_enumerator.MoveNext() == false)
			{
				if (_onComplete != null)
					_onComplete();
				
				return false;
			}
			return true;
		}
		
		public void Reset()
		{}

        public void Reuse(IEnumerator enumerator)
        {
           if (enumerator is SingleTask)
				_enumerator = enumerator;
			else
			{
                _task.Reset();
				_task.Add(enumerator);
				_enumerator = _task.GetEnumerator();
			}
			
			_onComplete = null;
        }


        IEnumerator		        _enumerator;
        SerialTaskCollection	_task;
		Action 	                _onComplete;
	}
}

