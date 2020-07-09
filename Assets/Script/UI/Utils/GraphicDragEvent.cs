using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicDragEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action callBack;

        [SerializeField] private Axis axis;

        [SerializeField] private bool reverse;

        [SerializeField] private float spring;

        private Vector2 vector;

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
                callBack?.Invoke();
            }
        }

        public enum Axis
        {
            Horizontal,
            Vertical,
        }
    }
}