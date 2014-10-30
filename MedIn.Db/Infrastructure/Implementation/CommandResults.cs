using System.Collections.Generic;
using System.Linq;

namespace MedIn.Db.Infrastructure.Implementation
{ 
    public class CommandResults : ICommandResults
    {
        private readonly List<ICommandResult> _results = new List<ICommandResult>();

        public void AddResult(ICommandResult result)
        {
            _results.Add(result);
        }

        public ICommandResult[] Results
        {
            get
            {
                return _results.ToArray();
            }
        }

        public bool Success
        {
            get
            {
                return _results.All(result => result.Success);
            }
        }
    }
}

