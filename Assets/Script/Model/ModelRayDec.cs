using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Model
{
    /// <summary>
    /// 射线选择目标，检测途中是否有障碍
    /// </summary>
    public class ModelRayDetection : MonoBehaviour
    {
        public Action<bool, List<Vector3>> callback;

        public Camera viewer;

        [SerializeField, Range(2, 100)] private int step = 2;

        [SerializeField, Range(2, 100)] private float height = 10f;

        [SerializeField] private float range = 100f;

        private bool pass;

        private Ray ray;

        private RaycastHit hit;

        private Vector3 _origination, _destination;

        private Vector3 _vector;

        private Vector3 _range = new Vector3(0, 0, 1);

        private readonly List<Vector3> line = new List<Vector3>();

        private void Awake()
        {
            if (viewer == null)
            {
                viewer = Camera.main;
            }
            Origination = Vector3.zero;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
#if UNITY_EDITOR
                if (!EventSystem.current.IsPointerOverGameObject())
#else
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
                {
                    Ray();
                }
            }
        }

        private void Ray()
        {
            ray = viewer.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 vector = hit.point - Origination;

                float distance = vector.magnitude;

                if (distance < range)
                {
                    Destination = hit.point;
                }
                else
                {
                    float ratio = range / distance;

                    Destination = Origination + vector * ratio;
                }
            }
        }

        private void Detection()
        {
            pass = true;

            line.Clear();

            float count = step - 1;

            Vector3 apex = (Origination + Destination) * 0.5f + (Vector3.up * height);

            for (int i = 0; i < step; i++)
            {
                line.Add(BezierPoint(i / count, Origination, apex, Destination));
            }
            for (int i = 0; i < count; i++)
            {
                if (Physics.Raycast(line[i], line[i + 1] - line[i], Vector3.Distance(line[i], line[i + 1])))
                {
                    pass = false;
                    break;
                }
            }
        }

        private Vector3 BezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
        {
            return (1 - t) * (1 - t) * start +
                2 * t * (1 - t) * center +
                t * t * end;
        }

        private void Excute()
        {
            Detection();

            callback?.Invoke(pass, line);
        }

        public Vector3 Origination
        {
            get
            {
                return _origination;
            }
            set
            {
                if (value.y < 2)
                {
                    value.y = 2;
                }
                _origination = value; Excute();
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
                if (value.y < 2)
                {
                    value.y = 2;
                }
                _destination = value; Excute();
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

        public Vector3 Range
        {
            get
            {
                if (_range.x != range * 2)
                {
                    _range.x = _range.y = range * 2;
                }
                return _range;
            }
        }
    }
}