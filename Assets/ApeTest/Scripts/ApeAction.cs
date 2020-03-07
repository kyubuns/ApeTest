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
        Execute,
        DontExecute,
    }
}

