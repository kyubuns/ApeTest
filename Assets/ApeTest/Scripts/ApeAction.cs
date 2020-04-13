using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApeTest
{
    public interface IApeAction
    {
        State CheckState();
        Task Run(CancellationToken cancellationToken);
    }

    public enum State
    {
        ExecuteForce,
        Execute,
        DontExecute,
    }

    public class ApeTestFinishException : Exception
    {
        public ApeTestFinishException(string message) : base(message)
        {
        }
    }
}

