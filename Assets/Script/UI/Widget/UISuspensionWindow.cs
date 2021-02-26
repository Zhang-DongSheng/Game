using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Graphic))]
    public class UISuspensionWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
    {
        enum Anchors
        {
            None,
            Horzontal,
            Vertical,
            Auto,
            Right,
        }

        [SerializeField] private Anchors anchor;

        [SerializeField] private RectTransform root;

        [SerializeField] private RectTransform target;

        [SerializeField] private Vector4 boundary;

        private Vector4 space;

        private Vector2 center;

        private Vector2 shift;

        private Vector2 position;

        public UnityEvent onClick;

        public UnityEvent onHoming;

        private void Awake()
        {
            if (target == null)
                target = GetComponent<RectTransform>();
            if (root == null)
                root = target.parent.GetComponent<RectTransform>();
        }

        private void Start()
        {
            space = new Vector4(0 + boundary.x, root.rect.width - boundary.y,
                0 + boundary.z, root.rect.height - boundary.w);
            center = new Vector2(root.rect.width / 2f, root.rect.height / 2f);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            shift += eventData.delta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(root, Input.mousePosition, null, out position);

            SetPosition(position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(root, Input.mousePosition, null, out position);

            Homing(position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            shift = Vector2.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (shift != Vector2.zero) return;

            onClick?.Invoke();
        }

        private void Homing(Vector2 position)
        {
            switch (anchor)
            {
                case Anchors.Horzontal:
                    position.x = position.x > center.x ? space.y : space.x;
                    break;
                case Anchors.Vertical:
                    position.y = position.y > center.y ? space.w : space.z;
                    break;
                case Anchors.Auto:
                    if (Math.Min(Math.Abs(position.x - space.x), Math.Abs(position.x - space.y)) >
                        Math.Min(Math.Abs(position.y - space.z), Math.Abs(position.y - space.w)))
                    {
                        goto case Anchors.Vertical;
                    }
                    else
                    {
                        goto case Anchors.Horzontal;
                    }
                case Anchors.Right:
                    position.x = space.y;
                    break;
            }

            SetPosition(position);

            onHoming?.Invoke();
        }

        private void SetPosition(Vector2 position)
        {
            if (position.x < space.x)
            {
                position.x = space.x;
            }
            else if (position.x > space.y)
            {
                position.x = space.y;
            }

            if (position.y < space.z)
            {
                position.y = space.z;
            }
            else if (position.y > space.w)
            {
                position.y = space.w;
            }

            if (target != null)
            {
                target.anchoredPosition = position;
            }
        }
    }
}