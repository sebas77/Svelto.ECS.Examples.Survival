using System.Collections.Generic;

namespace Svelto.Command.Dispatcher
{
	internal class EventDispatcher: IEventDispatcher
	{
		private Dictionary<object, List<ICommand>> _eventTable = new Dictionary<object, List<ICommand>>();

		public void Add<T>(T signal, ICommand action)
		{
			if (!_eventTable.ContainsKey(signal))
				_eventTable.Add(signal, new List <ICommand>());
	
			_eventTable[signal].Add(action);
		}
	
		public void Remove<T>(T signal, ICommand action)
		{
	        if (_eventTable.ContainsKey(signal))
			{
	            _eventTable[signal].Remove(action);
	
				if (_eventTable[signal] == null)
					_eventTable.Remove(signal);
			}
		}
		
		public void Dispatch<T, U>(T signal, U notification)
	    {
	        List<ICommand> cmds;
	        
	        if (_eventTable.TryGetValue(signal, out cmds))
	        {
	            foreach (ICommand cmd in cmds)
				{
					if ((cmd is IMultiInjectableCommand))
						(cmd as IMultiInjectableCommand).Inject(new object[] {notification});
					else
					if ((cmd is IInjectableCommand))
						(cmd as IInjectableCommand).Inject(notification);

					cmd.Execute();
				}
	        }
	    }
		
		public void Dispatch<T>(T signal, params object[] notification)
	    {
	        List<ICommand> cmds;
	        
	        if (_eventTable.TryGetValue(signal, out cmds))
	        {
	            foreach (ICommand cmd in cmds)
				{
					if ((cmd is IMultiInjectableCommand))
						(cmd as IMultiInjectableCommand).Inject(notification);
					else
					if ((cmd is IInjectableCommand))
						(cmd as IInjectableCommand).Inject(notification[0]);

					cmd.Execute();
				}
	        }
	    }
	    
	    public void Dispatch<T>(T signal)
	    {
	        List<ICommand> cmds;
	        
	        if (_eventTable.TryGetValue(signal, out cmds))
	        	cmds.ForEach((obj) => {obj.Execute();});
	    }
	}
}
