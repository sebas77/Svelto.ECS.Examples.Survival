#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Svelto.Tasks
{
	public class WWWEnumerator:IEnumerator
	{
		WWW _www;
		
		public WWWEnumerator(WWW www)
		{
			_www = www;
		}
		
		public object Current	{ get { return _www; }}
		
		public bool MoveNext ()
		{
			return _www.isDone == false;
		}
		
		public void Reset ()
		{
		}
	}

	public class ParallelTaskCollection: TaskCollection
    {
        public event Action		onComplete;

        override public float progress { get { return _progress + _subprogress; } }
 
        public ParallelTaskCollection():base()
        {
			_listOfStacks = new List<Stack<IEnumerator>>();
		}
		
		override public IEnumerator GetEnumerator()
		{
			isRunning = true;
			
			_listOfStacks.Clear();
			
			foreach (IEnumerator enumerator in registeredEnumerators)
            {
				Stack<IEnumerator> stack = new Stack<IEnumerator>();
				
				stack.Push(enumerator);
				
				_listOfStacks.Add(stack);
			}
			
			while (_listOfStacks.Count > 0)
			{
                _subprogress = 0;

                for (int i = 0; i < _listOfStacks.Count; ++i)
                {
                    Stack<IEnumerator> stack = _listOfStacks[i];

                    if (stack.Count > 0)
                    {
                        IEnumerator ce = stack.Peek(); //without popping it.

                        if (ce is EnumeratorWithProgress)
                            _subprogress += (ce as EnumeratorWithProgress).progress;
                    }
                }

                _subprogress /= (float)registeredEnumerators.Count;

				for (int i = 0; i < _listOfStacks.Count; ++i)
				{
					Stack<IEnumerator> stack = _listOfStacks[i];
					
					if (stack.Count > 0)
		            {
		                IEnumerator ce = stack.Peek(); //without popping it.

                        if (ce.MoveNext() == false)
                        {
                            stack.Pop(); //now it can be popped
                        }
                        else //ok the iteration is not over
                        {
                            if (ce.Current != null && ce.Current != ce)
                            {
                                if (ce.Current is IEnumerable)	//what we got from the enumeration is an IEnumerable?
                                    stack.Push(((IEnumerable)ce.Current).GetEnumerator());
                                else
                                if (ce.Current is IEnumerator)	//what we got from the enumeration is an IEnumerator?
                                    stack.Push(ce.Current as IEnumerator);
                                else
                                if (ce.Current is WWW)
                                    stack.Push(new WWWEnumerator(ce.Current as WWW));
                                else
                             	if (ce.Current is YieldInstruction)
                                    yield return ce.Current; //YieldInstructions are special cases and must be handled by Unity. They cannot be wrapped and pushed into a stack and it will pause a parallel execution
                            }
                        }
		            }
					else
					{
						_listOfStacks[i] = _listOfStacks[_listOfStacks.Count - 1];
						_listOfStacks.RemoveAt(_listOfStacks.Count - 1);

                        _progress = (float)(registeredEnumerators.Count - _listOfStacks.Count) / (float)registeredEnumerators.Count;
                        _subprogress = 0.0f;

						i--;
					}
				}

                yield return null;
			}
			
			isRunning = false;
			
			if (onComplete != null)
				onComplete();
        }
		
		private float 					 _progress;
		private List<Stack<IEnumerator>> _listOfStacks;
        private float                    _subprogress;
    }
}
#endif
