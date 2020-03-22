using System;
using UnityEngine;

namespace ApeTest
{
    public interface ILogger
    {
        void ActionStart(IApeAction action);
        void ActionFinish(IApeAction action);
        void TestFinish(ApeTestFinishException exception);
        void UnityLog(string condition, string stacktrace, LogType type);
    }

    public interface ITriggerUnityLog
    {
        Action<string, string, LogType> OnUnityLog { get; set; }
    }

    public class DefaultLogger : ILogger, ITriggerUnityLog
    {
        public Action<string, string, LogType> OnUnityLog { get; set; }

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
            if (type != LogType.Log)
            {
                Debug.Log($"ApeTest | [{Time.frameCount}/{Time.time:0.00}] {type} {condition}");
            }
            OnUnityLog?.Invoke(condition, stacktrace, type);
        }
    }
}
