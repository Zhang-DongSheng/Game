using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollOverflow : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;

        [SerializeField, Range(0.1f, 1000f)] private float space = 1;

        [SerializeField, Range(0, 5f)] private float duration = 1;

        public UnityEvent onForward, onBack;

        public UnityEvent<bool> onLayout;

        private RectTransform viewport;

        private RectTransform content;

        private Overflow overflow;

        private float timer;

        private void Awake()
        {
            if (scroll == null)
                scroll = GetComponent<ScrollRect>();
            scroll.onValueChanged.AddListener(OnValueChanged);

            viewport = scroll.viewport;

            content = scroll.content;
        }

        private void Update()
        {
            if (overflow == Overflow.None) return;

            timer += Time.deltaTime;

            if (timer > duration)
            {
                switch (overflow)
                {
                    case Overflow.Forward:
                        onForward?.Invoke();
                        break;
                    case Overflow.Back:
                        onBack?.Invoke();
                        break;
                }
                timer = 0;
            }
        }

        public void Relayout()
        {
            StartCoroutine(Layout());
        }

        IEnumerator Layout()
        {
            yield return new WaitForEndOfFrame();

            bool overfolow = scroll.content.rect.width > scroll.viewport.rect.width;

            if (overfolow)
            {
                scroll.movementType = ScrollRect.MovementType.Elastic;
            }
            else
            {
                scroll.horizontalNormalizedPosition = 0;
                scroll.movementType = ScrollRect.MovementType.Clamped;
            }
            onLayout?.Invoke(overfolow);
        }

        private void OnValueChanged(Vector2 vector)
        {
            if (scroll.horizontal)
            {
                if (vector.x > 1 && -content.anchoredPosition.x + viewport.rect.width - space > content.rect.width)
                {
                    overflow = Overflow.Forward;
                }
                else if (vector.x < 0 && -content.anchoredPosition.x + space < 0)
                {
                    overflow = Overflow.Back;
                }
                else
                {
                    overflow = Overflow.None; timer = 0;
                }
            }
            else
            {
                if (vector.y < 0 && content.anchoredPosition.y + viewport.rect.height - space > content.rect.height)
                {
                    overflow = Overflow.Forward;
                }
                else if (vector.y > 1 && content.anchoredPosition.y + space < 0)
                {
                    overflow = Overflow.Back;
                }
                else
                {
                    overflow = Overflow.None; timer = 0;
                }
            }
        }

        enum Overflow : byte
        {
            None,
            Forward,
            Back,
        }
    }
}