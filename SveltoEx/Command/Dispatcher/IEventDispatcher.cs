using System;
using System.Collections;
using Svelto.Command;

namespace Svelto.Command.Dispatcher
{
	internal interface IEventDispatcher
	{
	    void Add<T>(T signal, ICommand action);
	    void Remove<T>(T signal, ICommand action);
		
	    void Dispatch<T>(T notification);
	    void Dispatch<T, U>(T signal, U notification);
		void Dispatch<T>(T signal, params object[] notification);
	}
}
