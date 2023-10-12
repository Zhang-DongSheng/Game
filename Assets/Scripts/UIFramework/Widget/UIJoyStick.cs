using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// 方向摇杆
    /// </summary>
    public class UIJoyStick : ItemBase
    {
        enum Status
        {
            None,
            OnEnter,
            OnStay,
            OnLeave,
        }

        [SerializeField] private Canvas canvas;

        [SerializeField] private RectTransform parent;

        [SerializeField] private Transform m_background;

        [SerializeField] private Transform m_foreground;

        [SerializeField] private Transform m_arrow;

        [SerializeField, Range(1, 100)] private float m_radius;

        [SerializeField] private bool force = true;

        public UnityEvent onEnter, onLeave;

        public UnityEvent<Vector2> onMove;

        private Status _status;
        private Status status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    switch (value)
                    {
                        case Status.OnEnter:
                            timer = Time.time;
                            break;
                        case Status.OnStay:
                            SetActive(true, point_pre);
                            break;
                        case Status.OnLeave:
                            SetActive(false, Vector3.zero);
                            break;
                        default:
                            break;
                    }
                    _status = value;
                }
            }
        }

        private Touch touch;

        private int touch_ID;

        private float timer;

        private Vector2 point_pre, point_now;

        private void Start()
        {
            if (canvas == null)
            {
                canvas = FindObjectOfType<Canvas>();
            }
            SetActive(false, Vector3.zero);
        }

        protected override void OnUpdate(float delta)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //实现左半屏触发
                    if (Input.mousePosition.x <= UIDefine.ScreenHalfWidth)
                    {
                        onEnter?.Invoke();
                        status = Status.OnEnter;
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (status == Status.OnEnter)
                {
                    //等待一帧，区别射线的触发
                    if (Time.time - timer > Time.deltaTime)
                    {
                        point_pre = Input.mousePosition;
                        status = Status.OnStay;
                    }
                }
                else if (status == Status.OnStay)
                {
                    point_now = Input.mousePosition;
                    Vector2 vector = GetPostion(point_now, point_pre);
                    Vector3 rotation = GetRotation(vector);
                    m_foreground.localEulerAngles = rotation;
                    m_arrow.localPosition = vector;

                    if (force)
                    {
                        onMove?.Invoke(vector / m_radius);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (status != Status.OnLeave)
                {
                    onLeave?.Invoke();
                    status = Status.OnLeave;
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    touch = Input.GetTouch(i);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                            {
                                if (touch.position.x <= UIConfig.ScreenHalfWidth)
                                {
                                    touch_ID = touch.fingerId;
                                    onEnter?.Invoke();
                                    status = Status.OnEnter;
                                }
                            }
                            break;
                        case TouchPhase.Stationary:
                            if (touch.fingerId == touch_ID)
                            {
                                if (status == Status.OnEnter)
                                {
                                    if (Time.time - timer > Time.deltaTime)
                                    {
                                        point_pre = touch.position;
                                        status = Status.OnStay;
                                    }
                                }
                                else if (status == Status.OnStay)
                                {
                                    point_now = touch.position;
                                    Vector2 vector = GetPostion(point_now, point_pre);
                                    Vector3 rotation = GetRotation(vector);
                                    m_foreground.localEulerAngles = rotation;
                                    m_arrow.localPosition = vector;

                                    if (force)
                                    {
                                        onMove?.Invoke(vector / m_radius);
                                    }
                                }
                            }
                            break;
                        case TouchPhase.Moved:
                            goto case TouchPhase.Stationary;
                        case TouchPhase.Canceled:
                            if (touch.fingerId == touch_ID)
                            {
                                if (status != Status.OnLeave)
                                {
                                    onLeave?.Invoke();
                                    status = Status.OnLeave;
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
            if (status != Status.OnLeave)
            {
                onLeave?.Invoke();
                status = Status.OnLeave;
            }
        }

        private void SetActive(bool state, Vector2 point)
        {
            if (state)
            {
                if (ScreentPointToUGUIPosition(parent, point, out Vector2 position))
                {
                    m_background.localPosition = position;
                }
            }
            if (m_background.gameObject.activeSelf != state)
            {
                m_background.gameObject.SetActive(state);
            }
        }

        private Vector2 GetPostion(Vector2 point_now, Vector2 point_pre)
        {
            Vector2 vector = point_now - point_pre;

            float ratio = 1;

            float distance = Vector2.Distance(point_now, point_pre);

            if (distance >= m_radius)
            {
                ratio = m_radius / distance;
            }
            return vector * ratio;
        }

        private Vector3 GetRotation(Vector2 position)
        {
            Vector3 dir = new Vector3(position.x, 0, position.y);

            Vector3 angle = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;

            return Vector3.forward * angle.y * -1f;
        }

        private bool ScreentPointToUGUIPosition(RectTransform parent, Vector2 point, out Vector2 position)
        {
            if (canvas == null)
            {
                position = Vector2.zero; return false;
            }
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != null)
            {
                return RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, point, canvas.worldCamera, out position);
            }
            else
            {
                return RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, point, null, out position);
            }
        }
    }
}