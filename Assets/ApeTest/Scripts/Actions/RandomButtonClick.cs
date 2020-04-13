using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApeTest.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ApeTest.Action
{
    public class RandomButtonClick : IApeAction
    {
        private Button _targetButton;
        private readonly Func<Button, bool> _condition;

        public RandomButtonClick()
        {
        }

        public RandomButtonClick(Func<Button, bool> condition)
        {
            _condition = condition;
        }

        public State CheckState()
        {
            var buttons = Object.FindObjectsOfType<Button>();
            _targetButton = buttons.Shuffle().FirstOrDefault(x => ApeUtil.CheckButtonClickable(x, _condition));
            return _targetButton != null ? State.Execute : State.DontExecute;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            ExecuteEvents.Execute
            (
                _targetButton.gameObject,
                new PointerEventData(EventSystem.current),
                ExecuteEvents.pointerClickHandler
            );
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"RandomButtonClick(target = {ApeUtil.GetFullName(_targetButton.gameObject)})";
        }
    }
}
