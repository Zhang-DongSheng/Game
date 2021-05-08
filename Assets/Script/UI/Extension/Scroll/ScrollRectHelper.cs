using Game.UI;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectHelper : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;

        [SerializeField, Range(0, 1000f)] private float space;

        [SerializeField, Range(0, 5f)] private float duration;

        [Space(10)]

        public UnityEvent onTop, onBottom;

        private Vector2 resolution;

        private float timer;

        private ScrollStatus status;

        private void Awake()
        {
            resolution = UIManager.Instance.Resolution;

            if (scroll == null)
                scroll = GetComponent<ScrollRect>();
            scroll.onValueChanged.AddListener(OnValueChanged);
        }

        private void Update()
        {
            if (status == ScrollStatus.Idle) return;

            timer += Time.deltaTime;

            if (timer > duration)
            {
                switch (status)
                {
                    case ScrollStatus.Top:
                    case ScrollStatus.Left:
                        onTop?.Invoke();
                        break;
                    case ScrollStatus.Bottom:
                    case ScrollStatus.Right:
                        onBottom?.Invoke();
                        break;
                }

                timer = 0;

                status = ScrollStatus.Idle;
            }
        }

        private void OnValueChanged(Vector2 value)
        {
            if (scroll.vertical)
            {
                if (value.y > 1 && OverTop)
                {
                    status = ScrollStatus.Top;
                }
                else if (value.y < 0 && OverBottom)
                {
                    status = ScrollStatus.Bottom;
                }
                else
                {
                    timer = 0; status = ScrollStatus.Idle;
                }
            }
            else
            {
                if (value.x > 1 && OverLeft)
                {
                    status = ScrollStatus.Left;
                }
                else if (value.x < 0 && OverRight)
                {
                    status = ScrollStatus.Right;
                }
                else
                {
                    timer = 0; status = ScrollStatus.Idle;
                }
            }
        }

        private bool OverTop
        {
            get
            {
                if (scroll.content.anchoredPosition.y < space * -1)
                {
                    return true;
                }
                return false;
            }
        }

        private bool OverBottom
        {
            get
            {
                if (scroll.content.anchoredPosition.y > scroll.content.rect.height - resolution.y + space)
                {
                    return true;
                }
                return false;
            }
        }

        private bool OverLeft
        {
            get
            {
                if (scroll.content.anchoredPosition.x < space * -1)
                {
                    return true;
                }
                return false;
            }
        }

        private bool OverRight
        {
            get
            {
                if (scroll.content.anchoredPosition.x > scroll.content.rect.width - resolution.x + space)
                {
                    return true;
                }
                return false;
            }
        }

        enum ScrollStatus
        {
            Idle,
            Top,
            Bottom,
            Left,
            Right,
        }
    }
}