using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// Scroll 移动到最前
        /// </summary>
        public static void ToFront(this ScrollRect scroll)
        {
            if (scroll.horizontal)
            {
                scroll.normalizedPosition = new Vector2(1, 0);
            }
            else if (scroll.vertical)
            {
                scroll.normalizedPosition = new Vector2(0, 1);
            }
        }
        /// <summary>
        /// Scroll 移动到最后
        /// </summary>
        public static void ToBack(this ScrollRect scroll)
        {
            scroll.normalizedPosition = new Vector2(0, 0);
        }
        /// <summary>
        /// 提层
        /// </summary>
        public static void LiftUpLayer(this Transform transform)
        {
            var canvas = transform.AddOrReplaceComponent<Canvas>();

            canvas.overrideSorting = true;
            
            canvas.sortingOrder = 100;

            transform.AddOrReplaceComponent<GraphicRaycaster>();
        }
        /// <summary>
        /// 降层
        /// </summary>
        public static void LiftDownLayer(this Transform transform)
        {
            transform.RemoveComponent<GraphicRaycaster>();

            transform.RemoveComponent<Canvas>();
        }
    }
}