using System;
using Svelto.Command;

namespace Svelto.PeersLinker
{
	public interface IPeerListener: IPeer
	{
		ICommand CanExecuteCommand(Type withNotification);
	}
}

