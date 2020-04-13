using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ApeTest.Action
{
    public class SmartButtonClick : IApeAction
    {
        private Button _targetButton;
        private readonly Func<Button, bool> _condition;
        public Dictionary<string, int> PushNum { get; } = new Dictionary<string, int>();

        public SmartButtonClick(Func<Button, bool> condition)
        {
            _condition = condition;
        }

        public State CheckState()
        {
            var buttons = Object.FindObjectsOfType<Button>();
            var maxNum = 0;
            var clickables = buttons.Where(x => ApeUtil.CheckButtonClickable(x, _condition)).Select(x =>
            {
                var fullName = ApeUtil.GetFullName(x.gameObject);
                if (!PushNum.ContainsKey(fullName)) PushNum.Add(fullName, 0);
                var num = PushNum[fullName] + 1;
                maxNum = Mathf.Max(maxNum, num * 2);
                return (x, num);
            }).ToArray();
            if (clickables.Length == 0) return State.DontExecute;

            var lot = clickables.Select(x => (x.x, maxNum - x.num)).ToArray();
            var total = lot.Sum(x => x.Item2);

            var r = UnityEngine.Random.Range(0, total);
            var index = -1;
            while (r >= 0)
            {
                index++;
                r -= lot[index].Item2;
            }
            _targetButton = lot[index].x;
            PushNum[ApeUtil.GetFullName(lot[index].x.gameObject)]++;

            return State.Execute;
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
            return $"SmartButtonClick(target = {ApeUtil.GetFullName(_targetButton.gameObject)})";
        }
    }
}
