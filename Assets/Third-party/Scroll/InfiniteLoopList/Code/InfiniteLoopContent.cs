using System.Collections.Generic;

namespace UnityEngine.UI.Scroll
{
    public class InfiniteLoopContent : MonoBehaviour
    {
        private readonly Vector2 half = new Vector2(0.5f, 0.5f);

        private enum Axis
        {
            Horizontal,
            Vertical,
        }

        [SerializeField] private Axis axis = Axis.Horizontal;

        [SerializeField] private Vector2 size = new Vector2(100, 100);

        [SerializeField] private Vector2 spacing = Vector2.zero;

        private Vector2 position, cell;

        private readonly List<RectTransform> childs = new List<RectTransform>();

        public Vector2 Cell
        {
            get
            {
                return size + spacing;
            }
        }

        public void Format()
        {
            position = Vector2.zero;

            cell = Cell;

            childs.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                {
                    childs.Add(transform.GetChild(i) as RectTransform);
                }
            }

            for (int i = 0; i < childs.Count; i++)
            {
                AdaptRectTransform(childs[i], position, size);

                switch (axis)
                {
                    case Axis.Horizontal:
                        position.x += cell.x;
                        break;
                    case Axis.Vertical:
                        position.y -= cell.y;
                        break;
                    default:
                        position += cell;
                        break;
                }
            }
        }

        public void Clear()
        {
            for (int i = transform.childCount - 1; i > -1; i--)
            {
                GameObject _temp = transform.GetChild(i).gameObject;
                if (_temp != null)
                {
                    GameObject.DestroyImmediate(_temp);
                }
            }
        }

        private void AdaptRectTransform(RectTransform rect, Vector2 position, Vector2 size)
        {
            if (rect != null)
            {
                rect.anchorMin = half;

                rect.anchorMax = half;

                rect.pivot = half;

                rect.anchoredPosition = position;

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
        }
    }
}