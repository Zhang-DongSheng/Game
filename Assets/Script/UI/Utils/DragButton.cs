using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class DragButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Axis axis;

        public bool reverse;

        public float spring;

        private Vector2 vector;

        public Action onClick;

        public void OnBeginDrag(PointerEventData eventData)
        {
            vector = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            vector += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            bool more = false;

            switch (axis)
            {
                case Axis.Horizontal:
                    more = reverse ? Mathf.Abs(vector.x) > spring : vector.x > spring;
                    break;
                case Axis.Vertical:
                    more = reverse ? Mathf.Abs(vector.y) > spring : vector.y > spring;
                    break;
            }

            if (more)
            {
                onClick?.Invoke();
            }
        }

        public enum Axis
        {
            Horizontal,
            Vertical,
        }
    }
}