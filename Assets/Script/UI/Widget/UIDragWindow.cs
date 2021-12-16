using System;
using UnityEngine;

namespace Game
{
    public class UIDragWindow : MonoBehaviour
    {
        public Func<bool> allow;

        [SerializeField] private RectTransform area;

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

        private void Awake()
        {
            if (full)
            {
                boundary = new Vector2(Screen.width, Screen.height) * 0.5f;
            }
            panel = new Window(target, boundary);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                position = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                if (!Allow) return;

                vector = (Vector2)Input.mousePosition - position;

                position = Input.mousePosition;

                Shift(vector);
            }
            else if (Input.mouseScrollDelta.y != 0)
            {
                Scale(Input.mouseScrollDelta.y);
            }
#else
            if (Input.GetMouseButton(0))
            {
                if (!Allow) return;

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

        protected bool Allow
        {
            get
            {
                if (allow != null)
                {
                    return allow.Invoke();
                }
                return true;
            }
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