using System;

namespace Svelto.Command
{
	public class DelegateCommand:ICommand
	{
		public delegate void ToExecute();
		
		public DelegateCommand (ToExecute lambda)
		{
			_lambda = lambda;	
		}
		
		public void Execute()
		{
			_lambda();
		}
		
		private ToExecute _lambda;
	}

    public class DelegateInjectableCommand<T>:IInjectableCommand
	{
        public delegate void Delegate(T item);

		public DelegateInjectableCommand (Delegate lambda)
		{
			_lambda = lambda;	
		}
		
		public void Execute()
		{
			_lambda(_parameter);
		}

        public ICommand Inject(object dependency)
        {
            _parameter = (T)dependency;

            return this;
        }

        Delegate    _lambda;
        T           _parameter;
	}
}

