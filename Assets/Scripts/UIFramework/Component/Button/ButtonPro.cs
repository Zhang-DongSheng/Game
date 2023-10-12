using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 双击长按按钮
    /// </summary>
    [RequireComponent(typeof(MaskableGraphic))]
    public class ButtonPro : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField, Range(0.1f, 5)] private float interval = 1;

        [SerializeField, Range(1, 5)] private int multiple = 1;

        public UnityEvent onEnter;

        public UnityEvent onClick;

        public UnityEvent onMultipleClick;

        public UnityEvent<int> onPress;

        public UnityEvent onLeave;

        private int press_number;

        private float press_timer;

        private int multiple_number;

        private float multiple_timer;

        private TouchStatus status;

        private void OnEnable()
        {
            status = TouchStatus.Exit;
        }

        private void Update()
        {
            switch (status)
            {
                case TouchStatus.Enter:
                    if (Time.time >= press_timer)
                    {
                        press_number++;

                        press_timer = Time.time + interval;

                        if (onPress != null)
                        {
                            onPress.Invoke(press_number);
                        }
                    }
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick.Invoke();
            }

            if (Time.time > multiple_timer)
            {
                multiple_number = 1;
            }
            else
            {
                multiple_number++;

                if (multiple_number >= multiple)
                {
                    onMultipleClick?.Invoke();
                }
            }
            multiple_timer = Time.time + interval;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            press_number = 0;

            press_timer = Time.time + interval;

            onEnter?.Invoke();

            status = TouchStatus.Enter;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (status != TouchStatus.Exit)
            {
                status = TouchStatus.Exit; onLeave?.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (status != TouchStatus.Exit)
            {
                status = TouchStatus.Exit; onLeave?.Invoke();
            }
        }

        enum TouchStatus
        {
            Enter,
            Exit,
        }
    }
}