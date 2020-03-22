using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApeTest.Utils;
using UnityEngine;

namespace ApeTest
{
    public class Ape : IDisposable
    {
        public bool Disposed { get; private set; }

        private readonly IApeAction[] _actions;
        private readonly ILogger _logger;
        private IApeAction _runningAction;
        private Task _runningTask;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _started;

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

        private void OnStart()
        {
            _started = true;
            Application.logMessageReceived += HandleLog;
        }

        public bool Update()
        {
            if (Disposed) return false;

            if (!_started)
            {
                OnStart();
            }

            RunningTaskFinishCheck();
            if (_runningTask != null) return true;

            var random = new List<IApeAction>();

            try
            {
                foreach (var action in _actions)
                {
                    var state = action.CheckState();
                    if (state == State.Execute) random.Add(action);
                }
            }
            catch (ApeTestFinishException e)
            {
                _logger.TestFinish(e);
                DisposeInternal();
                return false;
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

            return true;
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

        private void HandleLog(string condition, string stacktrace, LogType type)
        {
            _logger.UnityLog(condition, stacktrace, type);
        }

        public void Dispose()
        {
            if (Disposed) return;

            _logger.TestFinish(new ApeTestFinishException("Disposed"));
            DisposeInternal();
        }

        public void DisposeInternal()
        {
            if (Disposed) return;

            Disposed = true;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            if (_logger != null && _logger is IDisposable disposableLogger) disposableLogger.Dispose();
            foreach (var action in _actions)
            {
                if (action != null && action is IDisposable disposableAction) disposableAction.Dispose();
            }

            if (_started)
            {
                Application.logMessageReceived -= HandleLog;
            }
        }
    }
}
