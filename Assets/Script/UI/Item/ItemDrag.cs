using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemDrag : ItemBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Axis axis;

        [Space(10)]

        public UnityEvent onBeginDrag;

        public UnityEvent<Vector2> onDrag;

        public UnityEvent onEndDrag;

        private bool drag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    drag = ScrollUtils.Horizontal(eventData.delta);
                    break;
                case Axis.Vertical:
                    drag = ScrollUtils.Vertical(eventData.delta);
                    break;
                default:
                    drag = true;
                    break;
            }

            if (drag)
            {
                onBeginDrag?.Invoke();

            }
            else
            {

            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (drag)
            {
                onDrag?.Invoke(eventData.delta);
            }
            else
            {

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (drag)
            {
                onEndDrag?.Invoke();
            }
            else
            {

            }
        }

        enum Axis
        {
            None,
            Horizontal,
            Vertical,
        }
    }
}