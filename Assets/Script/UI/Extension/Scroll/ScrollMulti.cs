﻿using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 叠加滑动列表
    /// </summary>
    public class ScrollMulti : ScrollRect, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ScrollRect scroll;

        [SerializeField] private bool multi;

        private bool drag;

        protected override void Awake()
        {
            base.Awake();

            if (scroll == null)
                scroll = GetMultiScroll();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;

            if (multi && scroll != null)
            {
                if (horizontal)
                {
                    drag = ScrollUtils.Horizontal(eventData.delta);
                }
                else if (vertical)
                {
                    drag = ScrollUtils.Vertical(eventData.delta);
                }
            }

            if (drag)
            {
                base.OnBeginDrag(eventData);
            }
            else
            {
                scroll.OnBeginDrag(eventData);
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
                scroll.OnDrag(eventData);
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
                scroll.OnEndDrag(eventData);
            }
        }

        private ScrollRect GetMultiScroll()
        {
            ScrollRect[] scrolls = GetComponentsInParent<ScrollRect>();

            for (int i = 0; i < scrolls.Length; i++)
            {
                if (i == 1)
                {
                    return scrolls[i];
                }
            }
            return null;
        }
    }
}