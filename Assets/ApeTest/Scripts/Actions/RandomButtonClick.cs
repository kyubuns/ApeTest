using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApeTest.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ApeTest.Action
{
    public class RandomButtonClick : IApeAction
    {
        private Button _targetButton;

        public State CheckState()
        {
            var buttons = Object.FindObjectsOfType<Button>();
            _targetButton = buttons.Shuffle().FirstOrDefault(CheckButtonClickable);
            return _targetButton != null ? State.Execute : State.DontExecute;
        }

        public static bool CheckButtonClickable(Button button)
        {
            if (!button.isActiveAndEnabled) return false;
            if (!button.interactable) return false;

            var rect = button.GetComponent<RectTransform>();
            var center = rect.position;
            var canvas = button.GetComponentInParent<Canvas>();
            var pos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, center);

            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = pos
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            var raycastResult = results.Any() && results[0].gameObject.GetComponentInParent<Button>() == button;

            return raycastResult;
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
            return $"RandomButtonClick(target = {_targetButton.name})";
        }
    }
}
