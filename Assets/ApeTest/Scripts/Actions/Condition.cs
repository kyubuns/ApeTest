using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApeTest.Action
{
    public class Condition : IApeAction
    {
        private readonly Func<State> _func;
        private readonly IApeAction _action;

        public Condition(Func<State> func, IApeAction action)
        {
            _func = func;
            _action = action;
        }

        public State CheckState()
        {
            var s = _func();
            if (s != State.Execute) return s;
            return _action.CheckState();
        }

        public Task Run(CancellationToken cancellationToken)
        {
            return _action.Run(cancellationToken);
        }
    }
}
