using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class ToggleDrag : MonoBehaviour,IBeginDragHandler , IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private float speed;

        [SerializeField] private List<Vector2> points;

        private Vector3 space;

        private Vector2 origination;

        private Vector2 destination;

        private Vector2 position;

        private Vector2 shift;

        private Status status;

        private float step;

        public UnityEvent<float> onValueChanged;

        public UnityEvent<int> onComplete;

        private void Awake()
        {
            space.x = points[0].x;

            space.y = points[points.Count - 1].x;

            space.z = space.y - space.x;

            origination.x = points[0].x;

            origination.y = points[0].y;
        }

        private void Update()
        {
            if (status == Status.Align)
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            status = Status.Drag;

            shift = Vector2.zero;

            position = target.anchoredPosition;
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
            Finish(index);
        }

        public void OnClick(int index)
        {
            if (shift != Vector2.zero) return;

            Finish(index);
        }

        public void FixedPosition(float value)
        {
            if (status == Status.Drag) return;

            position.x = value * space.z;

            position.x += space.y;

            position.y = origination.y;

            target.anchoredPosition = position;
        }

        private void Finish(int index)
        {
            destination = points[index];

            onValueChanged?.Invoke(index);

            shift = Vector2.zero;

            step = 0;

            status = Status.Align;
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
            position.y = origination.y;

            target.anchoredPosition = position;

            float current = space.x - position.x;

            onValueChanged?.Invoke(current / space.z);
        }

        enum Status
        {
            None,
            Drag,
            Align,
        }
    }
}