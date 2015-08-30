using System;
using System.Collections.Generic;

namespace Svelto.Command
{
    class CommandPool
    {
        Dictionary<System.Type, ICommand> _command = new Dictionary<System.Type, ICommand>();

        public TCommand GetCommand<TCommand>(Action<ICommand> onNewCommand) where TCommand:ICommand, new()
        {
            System.Type type = typeof(TCommand);

            ICommand command = null;

            if (_command.TryGetValue(type, out command) == true)
                return (TCommand)command;
            
            command = new TCommand();

            onNewCommand(command);

            _command[type] = command;

            return (TCommand)command;
        }
    }
}
