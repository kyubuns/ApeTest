using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApeTest.Utils;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ApeTest.Action
{
    public class RandomScroll : IApeAction
    {
        private ScrollRect _targetScroll;
        private readonly Func<ScrollRect, bool> _condition;

        public RandomScroll()
        {
        }

        public RandomScroll(Func<ScrollRect, bool> condition)
        {
            _condition = condition;
        }

        public State CheckState()
        {
            var scrollRects = Object.FindObjectsOfType<ScrollRect>();
            _targetScroll = scrollRects.Shuffle().FirstOrDefault(x => ApeUtil.CheckScrollRectClickable(x, _condition));
            return _targetScroll != null ? State.Execute : State.DontExecute;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            _targetScroll.verticalNormalizedPosition = UnityEngine.Random.Range(0.0f, 1.0f);
            _targetScroll.horizontalNormalizedPosition = UnityEngine.Random.Range(0.0f, 1.0f);
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"RandomScroll(target = {ApeUtil.GetFullName(_targetScroll.gameObject)})";
        }
    }
}
