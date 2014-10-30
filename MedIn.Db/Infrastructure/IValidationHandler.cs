using System.Collections.Generic;

namespace MedIn.Db.Infrastructure
{
    public interface IValidationHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<ValidationResult>  Validate(TCommand command);
    }
}
