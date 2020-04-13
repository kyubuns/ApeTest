using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ApeTest
{
    public static class ApeUtil
    {
        public static bool CheckScrollRectClickable(ScrollRect target, Func<ScrollRect, bool> condition = null)
        {
            if (!target.isActiveAndEnabled) return false;
            if (EventSystem.current == null) return false;
            if (EventSystem.current.enabled == false) return false;

            var rect = target.GetComponent<RectTransform>();
            var center = rect.position;
            var canvas = target.GetComponentInParent<Canvas>();
            var pos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, center);

            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = pos
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            var raycastResult = results.Any() && results[0].gameObject.GetComponentInParent<ScrollRect>() == target;

            if (raycastResult && condition != null)
            {
                raycastResult = condition(results[0].gameObject.GetComponentInParent<ScrollRect>());
            }

            return raycastResult;
        }

        public static bool CheckButtonClickable(Button button, Func<Button, bool> condition = null)
        {
            if (!button.isActiveAndEnabled) return false;
            if (!button.interactable) return false;
            if (EventSystem.current == null) return false;
            if (EventSystem.current.enabled == false) return false;

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

            if (raycastResult && condition != null)
            {
                raycastResult = condition(results[0].gameObject.GetComponentInParent<Button>());
            }

            return raycastResult;
        }

        public static string GetFullName(GameObject target)
        {
            var fullName = target.name;
            var t = target.transform.parent;

            while (t != null)
            {
                fullName = $"{t.name}/{fullName}";
                t = t.parent;
            }
            return fullName;
        }
    }
}
