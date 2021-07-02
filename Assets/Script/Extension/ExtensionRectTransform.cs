using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 全屏
        /// </summary>
        public static void Full(this RectTransform target)
        {
            target.anchorMin = Vector2.zero;

            target.anchorMax = Vector2.one;

            target.pivot = Vector2.one * 0.5f;

            target.sizeDelta = Vector2.zero;

            target.anchoredPosition = Vector2.zero;
        }
        /// <summary>
        /// 复制
        /// </summary>
        public static void Copy(this RectTransform target, RectTransform from)
        {
            target.localScale = from.localScale;

            target.anchorMin = from.anchorMin;

            target.anchorMax = from.anchorMax;

            target.pivot = from.pivot;

            target.sizeDelta = from.sizeDelta;

            target.anchoredPosition3D = from.anchoredPosition3D;
        }
        /// <summary>
        /// 偏移
        /// </summary>
        public static Vector2 Offset(RectTransform target)
        {
            Vector2 offset = Vector2.zero;

            while (target != null)
            {
                if (target.TryGetComponent(out Canvas _))
                {
                    break;
                }
                else
                {
                    offset += (Vector2)target.localPosition;
                }
                target = target.parent.GetComponent<RectTransform>();
            }
            return offset;
        }
        /// <summary>
        /// 设置位置
        /// </summary>
        public static void SetPosition(this RectTransform target, Vector2 position)
        {
            target.anchoredPosition = position;
        }
        /// <summary>
        /// 设置大小
        /// </summary>
        public static void SetSize(this RectTransform target, Vector2 size)
        {
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
        /// <summary>
        /// 重置
        /// </summary>
        public static void Reset(this RectTransform target)
        {
            target.localPosition = Vector3.zero;

            target.localRotation = Quaternion.identity;

            target.localScale = Vector3.one;
        }
    }
}