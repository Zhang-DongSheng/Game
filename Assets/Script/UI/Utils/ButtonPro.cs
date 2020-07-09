using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class ButtonPro : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Action onEnter;
        public Action onDown;
        public Action onUp;
        public Action onExit;
        public Action onClick;
        public Action onDoubleClick;
        public Action<int> onPress;

        [SerializeField, Range(0, 1)] private float interval_press;
        [SerializeField, Range(0, 1)] private float interval_double;

        //Long Press
        private float press_timer;
        private int m_press_number;
        private bool _press;
        public bool press
        {
            get { return _press; }
            set
            {
                if (value)
                {
                    m_press_number = 0;
                    press_timer = Time.time + interval_press;
                }
                _press = value;
            }
        }

        //Double Click
        private float double_timer;
        private int m_double_number;

        private void OnEnable()
        {
            press = false;
        }

        private void OnDisable()
        {
            if (press)
            {
                if (onExit != null)
                {
                    onExit.Invoke();
                }
                press = false;
            }
        }

        private void Update()
        {
            if (press)
            {
                if (Time.time >= press_timer)
                {
                    m_press_number++;
                    press_timer = Time.time + interval_press;
                    if (onPress != null)
                    {
                        onPress.Invoke(m_press_number);
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick.Invoke();
            }
            switch (m_double_number)
            {
                case 0:
                    goto default;
                case 1:
                    if (Time.time - double_timer < interval_double)
                    {
                        m_double_number++;
                        if (onDoubleClick != null)
                        {
                            onDoubleClick.Invoke();
                        }
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                default:
                    m_double_number = 1;
                    double_timer = Time.time;
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
            {
                onEnter.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
            {
                onDown.Invoke();
            }
            press = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (press)
            {
                if (onUp != null)
                {
                    onUp.Invoke();
                }
                press = false;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (press)
            {
                if (onExit != null)
                {
                    onExit.Invoke();
                }
                press = false;
            }
        }
    }
}