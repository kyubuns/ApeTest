using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ApeTest
{
    public interface ILogger
    {
        void Log(string message);
        void ActionStart(IApeAction action);
        void ActionFinish(IApeAction action);
        void TestFinish(ApeTestFinishException exception);
        void UnityLog(string condition, string stacktrace, LogType type);
    }

    public interface ITriggerUnityLog
    {
        Action<string, string, LogType> OnUnityLog { get; set; }
    }

    public class LoggerList : ILogger
    {
        private List<ILogger> _loggers;

        public LoggerList(params ILogger[] loggers)
        {
            _loggers = loggers.ToList();
        }

        public void Add(params ILogger[] loggers)
        {
            _loggers.AddRange(loggers);
        }

        public void Log(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(message);
            }
        }

        public void ActionStart(IApeAction action)
        {
            foreach (var logger in _loggers)
            {
                logger.ActionStart(action);
            }
        }

        public void ActionFinish(IApeAction action)
        {
            foreach (var logger in _loggers)
            {
                logger.ActionFinish(action);
            }
        }

        public void TestFinish(ApeTestFinishException exception)
        {
            foreach (var logger in _loggers)
            {
                logger.TestFinish(exception);
            }
        }

        public void UnityLog(string condition, string stacktrace, LogType type)
        {
            foreach (var logger in _loggers)
            {
                logger.UnityLog(condition, stacktrace, type);
            }
        }
    }

    public class DefaultLogger : ILogger, ITriggerUnityLog
    {
        public Action<string, string, LogType> OnUnityLog { get; set; }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void ActionStart(IApeAction action)
        {
            Debug.Log($"ApeTest | [{Time.frameCount}/{Time.time:0.00}] {action}");
        }

        public void ActionFinish(IApeAction action)
        {
        }

        public void TestFinish(ApeTestFinishException exception)
        {
            Debug.Log($"ApeTest | [{Time.frameCount}/{Time.time:0.00}] Finish by {exception.Message}");
        }

        public void UnityLog(string condition, string stacktrace, LogType type)
        {
            if (type != LogType.Log && type != LogType.Warning)
            {
                Debug.Log($"ApeTest | [{Time.frameCount}/{Time.time:0.00}] {type} {condition}");
            }
            OnUnityLog?.Invoke(condition, stacktrace, type);
        }
    }
}
