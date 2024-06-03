using Game.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class SliderV2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform m_handler;

        private RectTransform m_content;

        private Camera m_camera;

        private Vector2 size;

        private Vector2 location;

        private Vector2 relative = new Vector2(0, 0);

        public UnityEvent<Vector2> onValueChanged;

        private void Awake()
        {
            m_content = GetComponent<RectTransform>();

            size = m_content.rect.size * 0.5f;

            m_camera = UIManager.Instance.Canvas.worldCamera;

            if (m_camera == null)
            {
                m_camera = Camera.main;
            }
        }

        private void OnDrag(Vector2 position)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_content, position, m_camera, out location))
            {
                location.x = Mathf.Clamp(location.x, -size.x, size.x);

                location.y = Mathf.Clamp(location.y, -size.y, size.y);

                relative.x = location.x / size.x;

                relative.y = location.y / size.y;

                onValueChanged?.Invoke(relative);

                m_handler.localPosition = location;
            }
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