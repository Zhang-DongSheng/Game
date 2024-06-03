using Game;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class SliderV2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform m_handler;

        private Vector2 size;

        private Vector2 offset;

        private Vector2 center;

        private Vector2 position;

        private Vector2 relative = new Vector2(0, 0);

        public UnityEvent<Vector2> onValueChanged;

        private void Start()
        {
            var transform = GetComponent<RectTransform>();

            size = transform.rect.size * 0.5f;

            center = transform.AbsolutePosition();
        }

        private void OnDrag(Vector2 position)
        {
            this.position = UIUtils.ScreentPointToUGUIPosition(position);

            this.position.x = Mathf.Clamp(this.position.x, center.x - size.x, center.x + size.x);

            this.position.y = Mathf.Clamp(this.position.y, center.y - size.y, center.y + size.y);

            this.position -= center;

            relative.x = this.position.x / size.x;

            relative.y = this.position.y / size.y;

            onValueChanged?.Invoke(relative);

            m_handler.localPosition = this.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDrag(eventData.pressPosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDrag(eventData.position);
        }
    }
}