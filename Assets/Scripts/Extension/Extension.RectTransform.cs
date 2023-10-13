using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private static Vector2 anchorMin;

        private static Vector2 anchorMax;

        private static Vector2 delta, offset;

        private static readonly Vector3[] corners = new Vector3[4];
        /// <summary>
        /// 获取大小
        /// </summary>
        public static Vector2 GetSize(this RectTransform transform)
        {
            return transform.rect.size;
        }
        /// <summary>
        /// 获取偏移
        /// </summary>
        public static Vector2 GetOffset(RectTransform transform)
        {
            offset = Vector2.zero;

            while (transform != null)
            {
                if (transform.TryGetComponent(out Canvas _))
                {
                    break;
                }
                else
                {
                    offset += (Vector2)transform.localPosition;

                    if (transform.parent is RectTransform parent)
                    {
                        transform = parent;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return offset;
        }
        /// <summary>
        /// 获取顶点坐标数组
        /// </summary>
        public static Vector3[] GetCorners(this RectTransform transform, bool local = true)
        {
            if (local)
            {
                transform.GetLocalCorners(corners);
            }
            else
            {
                transform.GetWorldCorners(corners);
            }
            return corners;
        }
        /// <summary>
        /// 设置大小
        /// </summary>
        public static void SetSize(this RectTransform transform, Vector2 size)
        {
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
        /// <summary>
        /// 设置锚点
        /// </summary>
        public static void SetAnchors(this RectTransform transform, TextAnchor anchor)
        {
            anchorMin = transform.anchorMin;
            anchorMax = transform.anchorMax;

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
            transform.anchorMin = anchorMin;
            transform.anchorMax = anchorMax;
        }
        /// <summary>
        /// 设置位置
        /// </summary>
        public static void SetAnchoredPosition(this RectTransform transform, Vector2 position)
        {
            transform.anchoredPosition = position;
        }
        /// <summary>
        /// 设置全屏
        /// </summary>
        public static void SetFull(this RectTransform transform, RectOffset offset = null)
        {
            anchorMin = transform.anchorMin;
            anchorMax = transform.anchorMax;

            anchorMin.x = anchorMin.y = 0;
            anchorMax.x = anchorMax.y = 1;

            transform.anchorMin = anchorMin;
            transform.anchorMax = anchorMax;

            if (offset != null)
            {
                delta = new Vector2()
                {
                    x = offset.left + offset.right,
                    y = offset.top + offset.bottom,
                };
                transform.sizeDelta = delta * -1f;

                position = new Vector2()
                {
                    x = offset.left - offset.right,
                    y = offset.bottom - offset.top,
                };
                transform.anchoredPosition = position * 0.5f;
            }
            else
            {
                transform.sizeDelta = Vector2.zero;
                transform.anchoredPosition = Vector2.zero;
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
        public static void Reset(this RectTransform transform)
        {
            transform.anchorMin = Vector2.one * Half;

            transform.anchorMax = Vector2.one * Half;

            transform.pivot = Vector2.one * Half;

            transform.anchoredPosition = Vector3.zero;

            transform.localRotation = Quaternion.identity;

            transform.localScale = Vector3.one;
        }
        /// <summary>
        /// 调整位置
        /// </summary>
        public static void AdjustPosition(this RectTransform transform, Vector2 position, RectTransform parent)
        {
            var space = new Vector2(parent.rect.width, parent.rect.height) * 0.5f;

            var cell = new Vector2(transform.rect.width, transform.rect.height) * 0.5f;

            if (position.x + cell.x > space.x)
            {
                position.x = space.x - cell.x;
            }
            else if (position.x - cell.x < -space.x)
            {
                position.x = -space.x + cell.x;
            }

            if (position.y + cell.y > space.y)
            {
                position.y = space.y - cell.y;
            }
            else if (position.y - cell.y < -space.y)
            {
                position.y = -space.y + cell.y;
            }
            transform.anchoredPosition = position;
        }
    }
}