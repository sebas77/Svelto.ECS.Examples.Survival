using System.Collections;

namespace Svelto.Tasks.Internal
{
	internal class AsyncTask: IEnumerator
	{
		public IAbstractTask 	task { get; private set; }
		public object			token { set; private get; }

		public AsyncTask(IAbstractTask task)
		{
			this.task = task;
			enumerator = Execute();
		}

		public object Current 		{ get { return this; } }

		/// <summary>
		/// Gets the enumerator and execute the task
		/// The task is meant to be executed once. It can be
		/// both synchronous or asynchronous.
		/// If synchronous it must set isDone to true
		/// if asynchronous it must set isDone once the async 
		/// call is done 
		/// </summary>
		/// <returns>
		/// The enumerator.
		/// </returns>
		public bool MoveNext()
		{
			return enumerator.MoveNext();
		}

		IEnumerator Execute()
		{
			if (task is ITask)
				((ITask)task).Execute();
			else
			if (task is ITaskChain)
				((ITaskChain)task).Execute(token);
			else
				throw new System.Exception("not supported task " + task.ToString());

			while (task.isDone == false)
				yield return task;
		}

		public void Reset()
		{
		}

		public override string ToString()
		{
			return task.ToString();
		}

		IEnumerator		enumerator;
	}
}

