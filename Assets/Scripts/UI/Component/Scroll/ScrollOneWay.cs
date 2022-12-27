using Game;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 叠加滑动列表
    /// </summary>
    public class ScrollOneWay : ScrollRect
    {
        public ScrollRect other;

        public bool multi;

        private bool drag;

        protected override void Awake()
        {
            if (other == null)
            {
                other = GetMultiScroll();
            }
            base.Awake();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;

            if (multi && other != null)
            {
                if (horizontal)
                {
                    drag = Utility.Vector.Horizontal(eventData.delta);
                }
                else if (vertical)
                {
                    drag = Utility.Vector.Vertical(eventData.delta);
                }
            }

            if (drag)
            {
                base.OnBeginDrag(eventData);
            }
            else
            {
                other.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (drag)
            {
                base.OnDrag(eventData);
            }
            else
            {
                other.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (drag)
            {
                base.OnEndDrag(eventData);
            }
            else
            {
                other.OnEndDrag(eventData);
            }
        }

        private ScrollRect GetMultiScroll()
        {
            Transform parent = transform.parent;

            while (parent != null)
            {
                if (parent.TryGetComponent(out ScrollRect scroll))
                {
                    return scroll;
                }
                else
                {
                    parent = parent.parent;
                }
            }
            return null;
        }
    }
}