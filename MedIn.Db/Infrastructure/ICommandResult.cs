namespace MedIn.Db.Infrastructure
{
    public interface ICommandResult
    {
    	object Result { get; }
        bool Success { get; }
    }
}

