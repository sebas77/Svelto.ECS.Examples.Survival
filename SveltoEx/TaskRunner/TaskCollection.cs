using Svelto.Tasks.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Svelto.Tasks
{
	abstract public class TaskCollection: IEnumerable
	{
		protected Queue<IEnumerator> 	registeredEnumerators { get; private set; }

		public bool						isRunning 		{ protected set; get; }
		public int						tasksRegistered { get { return registeredEnumerators.Count; } }
		
		abstract public 				float progress { get; }
		
		public TaskCollection()
		{
			registeredEnumerators = new Queue<IEnumerator>();
		}
		
		public IAbstractTask Add(IAbstractTask task)
		{
			if (task == null)
				throw new ArgumentNullException();
			
			Add(new AsyncTask(task));

			return task;
		}
		
		public void Add(IEnumerable enumerable)
		{
			if (enumerable is TaskCollection)
			{
				registeredEnumerators.Enqueue(new EnumeratorWithProgress(enumerable.GetEnumerator(), 
													() => (enumerable as TaskCollection).progress));
				
				if ((enumerable as TaskCollection).tasksRegistered == 0)
					Console.WriteLine("Avoid to register zero size collections");
			}
			else
				registeredEnumerators.Enqueue(enumerable.GetEnumerator());
					
			if (enumerable == null)
				throw new ArgumentNullException();
		}
 
		public void Add(IEnumerator enumerator)
		{
			if (enumerator == null)
				throw new ArgumentNullException();
			
			registeredEnumerators.Enqueue(enumerator);
		}

        public void Reset()
        {
            registeredEnumerators.Clear();
        }
		
		abstract public IEnumerator GetEnumerator();
	}
}

