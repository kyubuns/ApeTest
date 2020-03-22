using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApeTest.Utils;

namespace ApeTest
{
    public class Ape : IDisposable
    {
        private readonly IApeAction[] _actions;
        private readonly ILogger _logger;
        private IApeAction _runningAction;
        private Task _runningTask;
        private CancellationTokenSource _cancellationTokenSource;

        public Ape(IApeAction[] actions)
        {
            _actions = actions;
            _logger = new DefaultLogger();
        }

        public Ape(IApeAction[] actions, ILogger logger)
        {
            _actions = actions;
            _logger = logger;
        }

        public void Update()
        {
            RunningTaskFinishCheck();
            if (_runningTask != null) return;

            var random = new List<IApeAction>();

            foreach (var action in _actions)
            {
                var state = action.CheckState();
                if (state == State.Execute) random.Add(action);
            }

            var pickedAction = random.RandomPick();
            if (pickedAction != null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _logger.ActionStart(pickedAction);
                _runningAction = pickedAction;
                _runningTask = pickedAction.Run(_cancellationTokenSource.Token);
            }
            RunningTaskFinishCheck();
        }

        private void RunningTaskFinishCheck()
        {
            if (_runningTask != null)
            {
                if (_runningTask.Status == TaskStatus.RanToCompletion
                    || _runningTask.Status == TaskStatus.Canceled
                    || _runningTask.Status == TaskStatus.Faulted)
                {
                    _logger.ActionFinish(_runningAction);
                    _runningAction = null;
                    _runningTask = null;
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
            _runningTask?.Dispose();
        }
    }
}
