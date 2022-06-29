using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Model
{
    public class RayTouch : RuntimeBase
    {
        [Tooltip("相机")]
        [SerializeField] private Camera viewer;
        [Tooltip("模式")]
        [SerializeField] private InputMode mode = InputMode.Mouse;
        [Tooltip("拖拽速度")]
        [SerializeField, Range(1, 50)] private float speed = 20f;
        [Tooltip("拖拽区间")]
        [SerializeField, Range(100, 500)] private float space = 100f;
        [Tooltip("范围")]
        [SerializeField] private Vector2 range = new Vector2(100, 100);
        [Tooltip("层级")]
        [SerializeField] private LayerMask layer;

        public UnityEvent callback;

        private float distance;

        private bool drag;

        private int fingerId;

        private Touch touch;

        private Ray ray;

        private RaycastHit hit;

        private Vector3 _point;

        private Vector3 _vector, _segment, _delta;

        private Vector3 _range = new Vector3(1, 1, 1);

        private Vector3 _origination = new Vector3(0, 2, 0);

        private Vector3 _destination = new Vector3(0, 2, 0);

        private void Awake()
        {
            if (viewer == null)
            {
                viewer = Camera.main;
            }
        }

        protected override void OnUpdate(float delta)
        {
            switch (mode)
            {
                case InputMode.Mouse:
                    {
#if UNITY_EDITOR
                        if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject())
                        {
                            OnMouseEnter(Input.mousePosition); drag = true;
                        }
                        else if (drag && Input.GetMouseButton(0))
                        {
                            OnMouse(Input.mousePosition);
                        }
                        else if (drag)
                        {
                            drag = false;
                        }
#else
                        if (Input.touchCount > 0)
                        {
                            for (int i = 0; i < Input.touchCount; i++)
                            {
                                touch = Input.GetTouch(i);

                                switch (touch.phase)
                                {
                                    case TouchPhase.Began:
                                        if (!drag && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                                        {
                                            OnMouseEnter(touch.position);
                                            fingerId = touch.fingerId;
                                            drag = true;
                                        }
                                        break;
                                    case TouchPhase.Moved:
                                        {
                                            if (drag && touch.fingerId == fingerId)
                                            {
                                                OnMouse(touch.position);
                                            }
                                        }
                                        break;
                                    case TouchPhase.Stationary:
                                        break;
                                    case TouchPhase.Canceled:
                                    case TouchPhase.Ended:
                                        {
                                            if (touch.fingerId == fingerId)
                                            {
                                                fingerId = -1;
                                                drag = false;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
#endif
                    }
                    break;
                case InputMode.Raycast:
                    {
                        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                        {
                            if (!IsPointerOverGameObject())
                            {
                                OnRay(Input.mousePosition);
                            }
                        }
                    }
                    break;
            }
        }

        private void OnMouseEnter(Vector3 position)
        {
            _point = position;
        }

        private void OnMouse(Vector3 position)
        {
            _delta = position - _point;

            _delta.z = _delta.y;

            _delta.y = 0f;

            _delta *= speed;

            _point = position;

            if (_delta.magnitude > space)
            {
                _delta = _delta.normalized * space;
            }
            _segment = _destination + _delta * Time.deltaTime - _origination;

            DetectionRange(_segment);
        }

        private void OnRay(Vector3 position)
        {
            ray = viewer.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit))
            {
                _delta = hit.point - _destination;

                _segment = hit.point - _origination;

                DetectionRange(_segment);
            }
        }

        private bool IsPointerOverGameObject()
        {
            if (EventSystem.current != null)
            {
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
#else
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
                {
                    return true;
                }
            }
            return false;
        }

        private void DetectionRange(Vector3 segment)
        {
            float distance = segment.magnitude;

            float ratio = range.y > distance ? 1f : range.y / distance;

            Destination = _origination + segment * ratio;
        }

        private void Excute()
        {
            callback?.Invoke();
        }

        public Vector3 Origination
        {
            get
            {
                return _origination;
            }
            set
            {
                if (value.y != 2)
                {
                    value.y = 2;
                }
                _origination = value; Excute();

                if (Vector3.Distance(_origination, _destination) < range.x ||
                    Vector3.Distance(_origination, _destination) > range.y)
                {
                    Destination = _destination;
                }
            }
        }

        public Vector3 Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _vector = value - _origination;

                _vector.y = 0;

                distance = Mathf.Pow(_vector.x, 2) + Mathf.Pow(_vector.z, 2);
                //范围检测
                if (distance == 0)
                {
                    _vector = Vector3.forward;
                }
                else if (distance < Mathf.Pow(range.x, 2))
                {
                    _vector = _vector.normalized * range.x;
                }
                else if (distance > Mathf.Pow(range.y, 2))
                {
                    _vector = _vector.normalized * range.y;
                }
                //边界检测
                if (Physics.Raycast(_origination, _vector, out RaycastHit hit, _vector.magnitude, layer))
                {
                    _vector = hit.point - _origination;

                    if (_vector.magnitude < range.x)
                    {
                        _vector = _vector.normalized * range.x;
                    }
                }
                _destination = _origination + _vector; Excute();
            }
        }

        public Vector3 Delta
        {
            get
            {
                if (drag)
                {
                    return _delta;
                }
                return Vector3.zero;
            }
        }

        public Vector3 Range
        {
            get
            {
                if (_range.x != range.y * 2)
                {
                    _range.x = _range.y = range.y * 2;
                }
                return _range;
            }
            set
            {
                range = value;
            }
        }

        public float Radius
        {
            get
            {
                return range.y;
            }
        }

        public float Angle
        {
            get
            {
                _vector = _destination - _origination;

                _vector.y = _vector.z;

                return Vector2.SignedAngle(_vector, Vector2.up);
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = 2 + 10f * Mathf.Clamp(value, 0, 1);
            }
        }

        enum InputMode
        {
            None,
            Mouse,
            Raycast,
        }
    }
}