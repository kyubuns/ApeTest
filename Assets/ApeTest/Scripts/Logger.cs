using UnityEngine;

namespace ApeTest
{
    public interface ILogger
    {
        void ActionStart(IApeAction action);
        void ActionFinish(IApeAction action);
    }

    public class DefaultLogger : ILogger
    {
        public void ActionStart(IApeAction action)
        {
            Debug.Log($"[{Time.frameCount}|{Time.time:0.00}] {action}");
        }

        public void ActionFinish(IApeAction action)
        {
        }
    }
}
