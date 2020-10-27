using System.Collections.Generic;

namespace UnityEngine.UI
{
    public static class UILayout
    {
        private static readonly Vector2 Zero = new Vector2(0, 0);

        public static void Horizontal(List<RectTransform> parents, List<RectTransform> childs, float interval = 0)
        {
            float current = 0, length;

            int index = 0, surplus = -1;

            for (int i = 0; i < childs.Count; i++)
            {
                length = parents[index].rect.width;

                if (current + childs[i].rect.width > length)
                {
                    current = 0; index++;

                    if (parents.Count > index)
                    {
                        SetPosition(parents[index], childs[i], new Vector2(current, 0));
                    }
                    else
                    {
                        surplus = i;
                        break;
                    }
                }
                else
                {
                    if (parents.Count > index)
                    {
                        SetPosition(parents[index], childs[i], new Vector2(current, 0));
                    }
                    else
                    {
                        surplus = i;
                        break;
                    }
                    current += childs[i].rect.width + interval;
                }

                SetActive(childs[i], true);
            }

            if (surplus == -1) return;

            for (int i = surplus; i < childs.Count; i++)
            {
                if (childs[i] != null)
                {
                    SetActive(childs[i], false);
                }
            }
        }

        private static void SetPosition(RectTransform parent, RectTransform child, Vector2 position)
        {
            if (child.parent != parent)
            {
                child.SetParent(parent);
            }
            child.anchorMin = Zero;
            child.anchorMax = Zero;
            child.pivot = Zero;
            child.anchoredPosition = position;
            child.gameObject.SetActive(true);
        }

        private static void SetActive(RectTransform target, bool active)
        {
            if (target != null && target.gameObject.activeSelf != active)
            {
                target.gameObject.SetActive(active);
            }
        }
    }
}