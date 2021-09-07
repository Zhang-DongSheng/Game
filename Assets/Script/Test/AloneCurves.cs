using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AloneCurves : MonoBehaviour
    {
        [Tooltip("µ¥Î»s/m")]
        [SerializeField, Range(1, 1000)] private float speed = 10;

        [SerializeField, Range(1, 100)] private int ratio = 10;

        [SerializeField] private Transform target;

        private int index, count;

        private float step;

        private Status status;

        private Vector3 position;

        private readonly Vector3[] vectors = new Vector3[] { new Vector3(), new Vector3(), new Vector3() };

        private readonly List<CurvesPoint> curves = new List<CurvesPoint>();

        private readonly SimpleCurves simple = new SimpleCurves();

        private void Update()
        {
            switch (status)
            {
                case Status.Idle:
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            vectors[2] = new Vector3(Random.Range(-50, 50f), 0, 0);

                            vectors[2].y = 0;

                            status = Status.Ready;
                        }
                    }
                    break;
                case Status.Ready:
                    {
                        curves.Clear();

                        List<Vector3> points = simple.FetchCurves(vectors[0], vectors[2], speed, ratio);

                        if (points == null || points.Count == 0)
                        {
                            status = Status.Idle;
                            return;
                        }
                        count = points.Count;

                        for (int i = 1; i < count; i++)
                        {
                            curves.Add(new CurvesPoint()
                            {
                                index = i,
                                length = Vector3.Distance(points[i - 1], points[i]),
                                origination = points[i - 1],
                                destination = points[i],
                            });
                            curves[i - 1].min = i > 1 ? curves[i - 2].max : 0;
                            curves[i - 1].max = curves[i - 1].min + curves[i - 1].length;
                        }

                        index = 0;

                        count = curves.Count;

                        step = 0;

                        status = Status.Run;
                    }
                    break;
                case Status.Run:
                    {
                        step += speed * Time.deltaTime;

                        for (int i = index; i < count; i++)
                        {
                            if (curves[i].Exist(step))
                            {
                                position = curves[i].Position(step);
                                index = i;
                                break;
                            }
                        }

                        Translation();

                        if (step >= curves[count - 1].max)
                        {
                            status = Status.Complete;
                        }
                    }
                    break;
                case Status.Complete:
                    {
                        status = Status.Idle;
                    }
                    break;
            }
        }

        private void Translation()
        {
            target.position = position;
        }

        enum Status
        {
            Idle,
            Ready,
            Run,
            Complete,
        }
    }

    public class CurvesPoint
    {
        public int index;

        public float min, max;

        public float length;

        public Vector3 origination, destination;

        public bool Exist(float value)
        {
            return value >= min && value <= max;
        }

        public Vector3 Position(float value)
        {
            if (length > 0)
            {
                return Vector3.Lerp(origination, destination, (value - min) / length);
            }
            return origination;
        }
    }
}