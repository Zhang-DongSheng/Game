using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemConsoleDrag : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private Direction direction;

        private Vector2 position;

        public UnityEvent<Vector2> onDrag;

        private void Awake()
        {
            position = target.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            switch (direction)
            {
                case Direction.Horizontal:
                    position.x = eventData.position.x;
                    break;
                case Direction.Vertical:
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