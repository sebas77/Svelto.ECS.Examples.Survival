using Svelto.Command;
using System;
using System.Collections.Generic;

namespace Svelto.IoC.Extensions.Command
{
	internal class CommandFactory:ICommandFactory
	{
		[Inject] public IContainer container { set; private get; }
		
		public TCommand Build<TCommand>() where TCommand:ICommand, new()
		{
			TCommand command = new TCommand();
			
			container.Inject(command);
			
			return command;
		}

		public TCommand Build<TCommand>(params object[] args) where TCommand:ICommand
		{
			Type commandClass = typeof(TCommand);
			
			TCommand command = (TCommand)Activator.CreateInstance(commandClass, args);
			
			container.Inject(command);
			
			return command;
		}
		
		public TCommand Build<TCommand>(Func<ICommand> constructor) where TCommand:ICommand
		{
			TCommand command = (TCommand)constructor();
			
			container.Inject(command);
			
			return command;
		}

        public void Execute<TCommand>(Action<TCommand> constructor) where TCommand:ICommand, new()
		{
            TCommand command = _commandPool.GetCommand<TCommand>((cmd) => container.Inject(cmd));
            
            constructor(command);

            command.Execute();
		}

        public void Execute<TCommand>() where TCommand:ICommand, new()
		{
            TCommand command = _commandPool.GetCommand<TCommand>((cmd) => container.Inject(cmd));
            
            command.Execute();
		}

        CommandPool _commandPool = new CommandPool();
	}
}
