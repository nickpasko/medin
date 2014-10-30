namespace MedIn.Db.Infrastructure
{
    public interface ICommandHandler<in TCommand> where TCommand: ICommand
    {
        ICommandResult Execute(TCommand command);
    }
}

