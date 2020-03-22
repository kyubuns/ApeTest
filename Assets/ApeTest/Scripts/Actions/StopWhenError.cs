using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ApeTest.Action
{
    public class StopWhenError : IApeAction
    {
        private bool _errorCaused;
        private string _errorMessage;

        public StopWhenError(ITriggerUnityLog triggerUnityLog)
        {
            triggerUnityLog.OnUnityLog += (message, _, type) =>
            {
                if (type != LogType.Log && type != LogType.Assert)
                {
                    _errorCaused = true;
                    _errorMessage = message;
                }
            };
        }

        public State CheckState()
        {
            if (_errorCaused) throw new ApeTestFinishException($"Error Caused {_errorMessage}");
            return State.DontExecute;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
