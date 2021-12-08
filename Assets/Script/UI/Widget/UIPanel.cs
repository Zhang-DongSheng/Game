using UnityEngine;

namespace Game
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [SerializeField, Range(0.1f, 100)] private float speed = 10f;

        [SerializeField] private Vector2 size = new Vector3(1f, 3f);

        [SerializeField] private Vector2 boundary = new Vector2(500, 360);

        private float[] distance = new float[2] { 0, 0 };

        private Touch[] touch = new Touch[2];

        private Vector2 position;

        private Vector2 vector;

        private Panel panel;

        private void Awake()
        {
            if (target == null)
            {
                target = GetComponent<RectTransform>();
            }
            panel = new Panel(target, boundary);
        }

        private void OnValidate()
        {

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

            panel.Adapting();

            target.anchoredPosition = panel.position;

            target.localScale = new Vector3(panel.scale, panel.scale, 1);
        }
    }

    class Panel
    {
        public Vector2 position;

        public Vector2 offset;

        public Vector2 size;

        public float scale;

        public Vector2 boundary;

        public Panel(RectTransform target, Vector2 boundary)
        {
            position = target.anchoredPosition;

            scale = target.localScale.x;

            size = target.rect.size;

            size /= scale * 2f;

            this.boundary = boundary;
        }

        public void Adapting()
        {
            offset = Offset(position + size * scale);

            offset += Offset(position - size * scale);

            position += offset;
        }

        private Vector2 Offset(Vector2 point)
        {
            return Boundary(point) - point;
        }

        private Vector2 Boundary(Vector2 point)
        {
            point.x = Mathf.Clamp(point.x, -boundary.x, boundary.x);

            point.y = Mathf.Clamp(point.y, -boundary.y, boundary.y);

            return point;
        }
    }

    enum Status
    {
        None,
        Drag,
        Alignment,
        Finish,
    }
}