namespace MedIn.Db.Infrastructure.Implementation
{
    public class CommandResult : ICommandResult
    {
        public CommandResult(bool success, object result = null)
        {
            Success = success;
        	Result = result;
        }

		public object Result { get; protected set; }

    	public bool Success { get; protected set; }
    }
}

