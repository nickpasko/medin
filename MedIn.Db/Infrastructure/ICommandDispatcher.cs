using System.Collections.Generic;

namespace MedIn.Db.Infrastructure
{
    public interface ICommandDispatcher
    {
        ICommandResult Submit<TCommand>(TCommand command) where TCommand: ICommand;
        IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : ICommand;
    }
}

