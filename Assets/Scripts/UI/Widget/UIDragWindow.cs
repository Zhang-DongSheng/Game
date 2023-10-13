using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UIDragWindow : ItemBase
    {
        [SerializeField] public string dragTag;

        [SerializeField] private RectTransform target;

        [SerializeField, Range(0.1f, 100)] private float speed = 10f;

        [SerializeField] private bool full;

        [SerializeField] private Vector2 size = new Vector3(1f, 3f);

        [SerializeField] private Vector2 boundary = new Vector2(500, 360);

        private float[] distance = new float[2] { 0, 0 };

        private Touch[] touch = new Touch[2];

        private Vector2 position;

        private Vector2 vector;

        private Window panel;

        private bool drag;

        private void Start()
        {
            if (full)
            {
                boundary = new Vector2(Screen.width, Screen.height) * 0.5f;
            }
            panel = new Window(target, boundary);
        }

        protected override void OnUpdate(float delta)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                position = Input.mousePosition;

                if (IsPointerOverGameObject(position))
                {
                    drag = true;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (!drag) return;

                vector = (Vector2)Input.mousePosition - position;

                position = Input.mousePosition;

                Shift(vector);
            }
            else if (Input.mouseScrollDelta.y != 0)
            {
                Scale(Input.mouseScrollDelta.y);
            }
            else
            {
                drag = false;
            }
#else
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverGameObject(Input.mousePosition))
                {
                    drag = true;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (!drag) return;

                if (Input.touchCount == 1)
                {
                    touch[0] = Input.GetTouch(0);

                    if (touch[0].phase == TouchPhase.Moved)
                    {
                        Shift(touch[0].deltaPosition);
                    }
                }
                else if (Input.touchCount >= 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        touch[i] = Input.GetTouch(i);
                    }
                    distance[0] = Vector2.Distance(touch[0].position, touch[1].position);

                    if (touch[1].phase == TouchPhase.Began)
                    {
                        distance[1] = distance[0];
                    }
                    else
                    {
                        if (distance[0] != distance[1])
                        {
                            Scale(distance[0] - distance[1]);
                        }
                        distance[1] = distance[0];
                    }
                }
            }
            else
            {
                drag = false;
            }
#endif
        }

        protected void Shift(Vector2 delta)
        {
            panel.position += delta;

            panel.Adapting();

            target.anchoredPosition = panel.position;
        }

        protected void Scale(float delta)
        {
            panel.scale += delta * speed * Time.deltaTime;

            panel.scale = Mathf.Clamp(panel.scale, size.x, size.y);

            panel.position = panel.center * panel.scale;

            panel.Adapting();

            target.anchoredPosition = panel.position;

            target.localScale = new Vector3(panel.scale, panel.scale, 1);
        }

        private bool IsPointerOverGameObject(Vector2 mousePosition)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);

            eventData.position = mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                if (string.IsNullOrEmpty(dragTag))
                {
                    return true;
                }
                else if (raycastResults[0].gameObject.CompareTag(dragTag))
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Window
    {
        public Vector2 position;

        public Vector2 center;

        public Vector2 offset;

        public Vector2 size;

        public float scale;

        public float value;

        public Vector2 boundary;

        public bool inside;

        public Window(RectTransform target, Vector2 boundary)
        {
            position = target.anchoredPosition;

            scale = target.localScale.x;

            size = target.rect.size;

            size /= scale * 2f;

            offset = new Vector2();

            this.boundary = boundary;

            inside = boundary.x > size.x && boundary.y > size.y;
        }

        public void Adapting()
        {
            offset.x = offset.y = 0;

            if (inside)
            {
                Inside();
            }
            else
            {
                Outside();
            }
            position += offset; center = position / scale;
        }

        private void Inside()
        {
            if (position.x < 0)
            {
                value = position.x - size.x * scale;

                if (value < -boundary.x)
                {
                    offset.x -= value + boundary.x;
                }
            }
            else
            {
                value = position.x + size.x * scale;

                if (value > boundary.x)
                {
                    offset.x -= value - boundary.x;
                }
            }
            if (position.y < 0)
            {
                value = position.y - size.y * scale;

                if (value < -boundary.y)
                {
                    offset.y -= value + boundary.y;
                }
            }
            else
            {
                value = position.y + size.y * scale;

                if (value > boundary.y)
                {
                    offset.y -= value - boundary.y;
                }
            }
        }

        private void Outside()
        {
            if (position.x < 0)
            {
                value = position.x + size.x * scale;

                if (value < boundary.x)
                {
                    offset.x -= value - boundary.x;
                }
            }
            else
            {
                value = position.x - size.x * scale;

                if (value > -boundary.x)
                {
                    offset.x -= value + boundary.x;
                }
            }
            if (position.y < 0)
            {
                value = position.y + size.y * scale;

                if (value < boundary.y)
                {
                    offset.y -= value - boundary.y;
                }
            }
            else
            {
                value = position.y - size.y * scale;

                if (value > -boundary.y)
                {
                    offset.y -= value + boundary.y;
                }
            }
        }
    }
}