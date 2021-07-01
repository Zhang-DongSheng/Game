using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class DragToggle : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private float speed;

        [SerializeField] private List<Vector2> points;

        private Vector2 space;

        private Vector2 destination;

        private Vector2 position;

        private Vector2 shift;

        private Status status;

        private float step;

        public UnityEvent<int> onValueChanged;

        private void Awake()
        {
            space.x = points[0].x;

            space.y = points[points.Count - 1].x;
        }

        private void Update()
        {
            if (status == Status.Update)
            {
                step += Time.deltaTime * speed;

                position = Vector2.Lerp(position, destination, step);

                SetPosition(position);

                if (step > 1)
                {
                    status = Status.None;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            shift += eventData.delta;

            position += eventData.delta;

            SetPosition(position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (shift == Vector2.zero) return;

            int index = 0;

            float left, right;

            if (position.x >= space.x)
            {
                index = 0;
            }
            else if (position.x <= space.y)
            {
                index = points.Count - 1;
            }
            else
            {
                for (int i = 1; i < points.Count; i++)
                {
                    left = points[i - 1].x - position.x;

                    right = position.x - points[i].x;

                    if (left >= 0 && right > 0)
                    {
                        index = left < right ? i - 1 : i;
                        break;
                    }
                }
            }
            Complete(index);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            shift = Vector2.zero;

            position = target.anchoredPosition;
        }

        public void OnClick(int index)
        {
            if (shift != Vector2.zero) return;

            Complete(index);
        }

        private void Complete(int index)
        {
            destination = points[index];

            onValueChanged?.Invoke(index);

            shift = Vector2.zero;

            step = 0;

            status = Status.Update;
        }

        private void SetPosition(Vector2 position)
        {
            if (position.x > space.x)
            {
                position.x = space.x;
            }
            else if (position.x < space.y)
            {
                position.x = space.y;
            }
            position.y = 0;

            target.anchoredPosition = position;
        }

        enum Status
        {
            None,
            Update,
        }
    }
}