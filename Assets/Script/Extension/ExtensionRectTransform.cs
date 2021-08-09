using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        static private Vector2 anchorMin;

        static private Vector2 anchorMax;

        static private Vector2 delta;

        /// <summary>
        /// 全屏
        /// </summary>
        public static void Full(this RectTransform target, RectOffset offset = null)
        {
            anchorMin = target.anchorMin;
            anchorMax = target.anchorMax;

            anchorMin.x = anchorMin.y = 0;
            anchorMax.x = anchorMax.y = 1;

            target.anchorMin = anchorMin;
            target.anchorMax = anchorMax;

            if (offset != null)
            {
                delta = new Vector2()
                {
                    x = offset.left + offset.right,
                    y = offset.top + offset.bottom,
                };
                target.sizeDelta = delta * -1f;

                position = new Vector2()
                {
                    x = offset.left - offset.right,
                    y = offset.bottom - offset.top,
                };
                target.anchoredPosition = position * 0.5f;
            }
            else
            {
                target.sizeDelta = Vector2.zero;
                target.anchoredPosition = Vector2.zero;
            }
        }
        /// <summary>
        /// 偏移
        /// </summary>
        public static Vector2 CanvasDelta(RectTransform target)
        {
            delta = Vector2.zero;

            while (target != null)
            {
                if (target.TryGetComponent(out Canvas _))
                {
                    break;
                }
                else
                {
                    delta += (Vector2)target.localPosition;

                    if (target.parent is RectTransform parent)
                    {
                        target = parent;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return delta;
        }
        /// <summary>
        /// 设置锚点
        /// </summary>
        public static void SetAnchors(this RectTransform target, TextAnchor anchor)
        {
            anchorMin = target.anchorMin;
            anchorMax = target.anchorMax;

            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    anchorMin.x = anchorMax.x = 0;
                    anchorMin.y = anchorMax.y = 1;
                    break;
                case TextAnchor.UpperCenter:
                    anchorMin.x = anchorMax.x = 0.5f;
                    anchorMin.y = anchorMax.y = 1;
                    break;
                case TextAnchor.UpperRight:
                    anchorMin.x = anchorMax.x = 1;
                    anchorMin.y = anchorMax.y = 1;
                    break;
                case TextAnchor.MiddleLeft:
                    anchorMin.x = anchorMax.x = 0;
                    anchorMin.y = anchorMax.y = 0.5f;
                    break;
                case TextAnchor.MiddleCenter:
                    anchorMin.x = anchorMax.x = 0.5f;
                    anchorMin.y = anchorMax.y = 0.5f;
                    break;
                case TextAnchor.MiddleRight:
                    anchorMin.x = anchorMax.x = 1;
                    anchorMin.y = anchorMax.y = 0.5f;
                    break;
                case TextAnchor.LowerLeft:
                    anchorMin.x = anchorMax.x = 0;
                    anchorMin.y = anchorMax.y = 0;
                    break;
                case TextAnchor.LowerCenter:
                    anchorMin.x = anchorMax.x = 0.5f;
                    anchorMin.y = anchorMax.y = 0;
                    break;
                case TextAnchor.LowerRight:
                    anchorMin.x = anchorMax.x = 1;
                    anchorMin.y = anchorMax.y = 0;
                    break;
            }
            target.anchorMin = anchorMin;
            target.anchorMax = anchorMax;
        }
        /// <summary>
        /// 设置位置
        /// </summary>
        public static void SetAnchoredPosition(this RectTransform target, Vector2 position)
        {
            target.anchoredPosition = position;
        }
        /// <summary>
        /// 设置大小
        /// </summary>
        public static void SetSize(this RectTransform target, Vector2 size)
        {
            if (target.TryGetComponent(out RectTransform rect))
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
        }
        /// <summary>
        /// 复制
        /// </summary>
        public static void Copy(this RectTransform to, RectTransform from)
        {
            to.localScale = from.localScale;

            to.anchorMin = from.anchorMin;

            to.anchorMax = from.anchorMax;

            to.pivot = from.pivot;

            to.sizeDelta = from.sizeDelta;

            to.anchoredPosition3D = from.anchoredPosition3D;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public static void Reset(this RectTransform target)
        {
            target.anchorMin = Vector2.one * HALF;

            target.anchorMax = Vector2.one * HALF;

            target.pivot = Vector2.one * HALF;

            target.anchoredPosition = Vector3.zero;

            target.localRotation = Quaternion.identity;

            target.localScale = Vector3.one;
        }
    }
}