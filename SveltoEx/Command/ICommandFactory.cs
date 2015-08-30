using System;

namespace Svelto.Command
{
	internal interface ICommandFactory
	{
		TCommand Build<TCommand>() where TCommand:ICommand, new();
		TCommand Build<TCommand>(Func<ICommand> constructor) where TCommand:ICommand;
		TCommand Build<TCommand>(params object[] args) where TCommand:ICommand;

        void Execute<TCommand>(Action<TCommand> constructor) where TCommand : ICommand, new();
        void Execute<TCommand>() where TCommand : ICommand, new();
	}
}

