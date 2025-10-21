using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemConsoleDrag : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private Axis axis;

        private Vector2 position;

        public UnityEvent<Vector2> onDrag;

        private void Awake()
        {
            position = target.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    position.x = eventData.position.x;
                    break;
                case Axis.Vertical:
                    position.y = eventData.position.y;
                    break;
                default:
                    position = eventData.position;
                    break;
            }
            target.anchoredPosition = position;

            onDrag?.Invoke(position);
        }
    }
}