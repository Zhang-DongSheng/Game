using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 方向摇杆
    /// </summary>
    public class UIJoyStick : MonoBehaviour
    {
        enum JoyStickState
        {
            None,
            OnEnter,
            OnStay,
            OnLeave,
        }

        public Transform m_background;
        public Transform m_foreground;
        public Transform m_arrow;

        [Range(1, 100)]
        public float m_radius;

        public Action onEnter;
        public Action<Vector2> onMove;
        public Action onLeave;

        private JoyStickState _js_state;
        private JoyStickState js_state
        {
            get
            {
                return _js_state;
            }
            set
            {
                if (_js_state != value)
                {
                    switch (value)
                    {
                        case JoyStickState.OnEnter:
                            m_timer = Time.time;
                            break;
                        case JoyStickState.OnStay:
                            SetActive(true, point_pre);
                            break;
                        case JoyStickState.OnLeave:
                            SetActive(false, Vector3.zero);
                            break;
                        default:
                            break;
                    }
                    _js_state = value;
                }
            }
        }

        private Touch m_touch;
        private int m_touch_ID;

        private float m_timer;
        private Vector3 m_position;

        private Vector2 point_pre;
        private Vector2 point_now;

        public bool m_force = true;

        private void Awake()
        {
            SetActive(false, Vector3.zero);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //实现左半屏触发
                    if (Input.mousePosition.x <= UIConfig.ScreenHalfWidth)
                    {
                        if (onEnter != null)
                        {
                            onEnter();
                        }
                        js_state = JoyStickState.OnEnter;
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (js_state == JoyStickState.OnEnter)
                {
                    //等待一帧，区别射线的触发
                    if (Time.time - m_timer > Time.deltaTime)
                    {
                        point_pre = Input.mousePosition;
                        js_state = JoyStickState.OnStay;
                    }
                }
                else if (js_state == JoyStickState.OnStay)
                {
                    point_now = Input.mousePosition;
                    Vector2 vector = Get_Postion(point_now, point_pre);
                    Vector3 rotation = Get_Rotation(vector);
                    m_foreground.localEulerAngles = rotation;
                    m_arrow.localPosition = vector;

                    if (m_force && onMove != null)
                    {
                        onMove(vector / m_radius);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (js_state != JoyStickState.OnLeave)
                {
                    if (onLeave != null)
                    {
                        onLeave();
                    }
                    js_state = JoyStickState.OnLeave;
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                m_touch = Input.GetTouch(i);

                switch (m_touch.phase)
                {
                    case TouchPhase.Began:
                        if (!EventSystem.current.IsPointerOverGameObject(m_touch.fingerId))
                        {
                            if (m_touch.position.x <= UIConfig.ScreenHalfWidth)
                            {
                                m_touch_ID = m_touch.fingerId;
                                if (onEnter != null)
                                {
                                    onEnter();
                                }
                                js_state = JoyStickState.OnEnter;
                            }
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (m_touch.fingerId == m_touch_ID)
                        {
                            if (js_state == JoyStickState.OnEnter)
                            {
                                if (Time.time - m_timer > Time.deltaTime)
                                {
                                    point_pre = m_touch.position;
                                    js_state = JoyStickState.OnStay;
                                }
                            }
                            else if (js_state == JoyStickState.OnStay)
                            {
                                point_now = m_touch.position;
                                Vector2 vector = Get_Postion(point_now, point_pre);
                                Vector3 rotation = Get_Rotation(vector);
                                m_foreground.localEulerAngles = rotation;
                                m_arrow.localPosition = vector;

                                if (m_force && onMove != null)
                                {
                                    onMove(vector / m_radius);
                                }
                            }
                        }
                        break;
                    case TouchPhase.Moved:
                        goto case TouchPhase.Stationary;
                    case TouchPhase.Canceled:
                        if (m_touch.fingerId == m_touch_ID)
                        {
                            if (js_state != JoyStickState.OnLeave)
                            {
                                if (onLeave != null)
                                {
                                    onLeave();
                                }
                                js_state = JoyStickState.OnLeave;
                            }
                        }
                        break;
                    case TouchPhase.Ended:
                        goto case TouchPhase.Canceled;
                    default:
                        break;
                }
            }
        }
#else
        
#endif
        }

        /// <summary>
        ///强制中断操纵杆[优先级小于攻击]
        /// </summary>
        public void BreakeJoyStick()
        {
            if (js_state != JoyStickState.OnLeave)
            {
                if (onLeave != null)
                {
                    onLeave();
                }
                js_state = JoyStickState.OnLeave;
            }
        }

        private void SetActive(bool state, Vector3 pos)
        {
            if (state)
            {
                m_background.localPosition = UIUtils.PositionFormat(pos, Vector3.zero);
            }
            if (m_background.gameObject.activeSelf != state)
            {
                m_background.gameObject.SetActive(state);
            }
        }

        private Vector3 Get_Postion(Vector3 point_now, Vector3 point_pre)
        {
            Vector3 vector = point_now - point_pre;

            float ratio = 1;
            float distance = Vector3.Distance(point_now, point_pre);
            if (distance >= m_radius)
            {
                ratio = m_radius / distance;
            }

            return vector * ratio;
        }

        private Vector3 Get_Rotation(Vector2 position)
        {
            Vector3 dir = new Vector3(position.x, 0, position.y);
            Vector3 angle = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
            return Vector3.forward * angle.y * -1f;
        }
    }
}