using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIUtils
    {
        public static Vector2 ScreentPointToUGUIPosition(Vector2 position)
        {
            position.x -= Const.ScreenHalfWidth;

            position.y -= Const.ScreenHalfHeight;

            position.x *= Const.ScreenWidthRatio;

            position.y *= Const.ScreenHeightRatio;

            return position;
        }

        public static bool ScreenPointToLocalPointInRectangle(Canvas canvas, RectTransform parent, Vector2 point, out Vector2 position)
        {
            if (canvas == null)
            {
                position = Vector2.zero; return false;
            }
            // Unity API
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != null)
            {
                return RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, point, canvas.worldCamera, out position);
            }
            else
            {
                return RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, point, null, out position);
            }
        }

        public static bool IsPointerOverGameObjectWithTag(params string[] tags)
        {
            if (EventSystem.current != null)
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition,
                };
                List<RaycastResult> raycastResults = new List<RaycastResult>();

                EventSystem.current.RaycastAll(eventData, raycastResults);

                if (raycastResults.Count > 0)
                {
                    if (tags == null || tags.Length == 0)
                    {
                        return true;
                    }
                    else if (tags.Contains(raycastResults[0].gameObject.tag))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void Rebuild(RectTransform transform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
        }
    }
}