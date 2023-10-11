using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class RangeVector2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float min;

        [SerializeField] private float max = 1;

        [SerializeField] private bool whole;

        [SerializeField] private RectTransform content;

        [SerializeField] private RectTransform handle;

        private Vector2 normalize = Vector2.zero;

        private Vector2 value = Vector2.zero;

        private Vector2 position = Vector2.zero;

        public UnityEvent<Vector2> onValueChanged;

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnUpdate(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnUpdate(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnUpdate(eventData);
        }

        private void OnUpdate(PointerEventData eventData)
        {
            if (content == null || max <= min) return;

            float width = content.rect.width;

            float height = content.rect.height;

            if (width == 0 || height == 0) return;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(content, eventData.position, eventData.pressEventCamera, out position))
            {
                position.x += width * 0.5f;

                position.y += height * 0.5f;

                normalize.x = Mathf.Clamp01(position.x / width);

                normalize.y = Mathf.Clamp01(position.y / height);

                if (whole)
                {
                    value.x = Mathf.RoundToInt(Mathf.Lerp(min, max, normalize.x));

                    value.y = Mathf.RoundToInt(Mathf.Lerp(min, max, normalize.y));

                    normalize.x = (value.x - min) / (max - min);

                    normalize.y = (value.y - min) / (max - min);

                    position.x = Mathf.Lerp(0, width, normalize.x) - width * 0.5f;

                    position.y = Mathf.Lerp(0, height, normalize.y) - height * 0.5f;
                }
                else
                {
                    value.x = Mathf.Lerp(min, max, normalize.x);

                    value.y = Mathf.Lerp(min, max, normalize.y);

                    position.x = Mathf.Lerp(0, width, normalize.x) - width * 0.5f;

                    position.y = Mathf.Lerp(0, height, normalize.y) - height * 0.5f;
                }
                onValueChanged?.Invoke(value);

                handle.anchoredPosition = position;
            }
        }
    }
}