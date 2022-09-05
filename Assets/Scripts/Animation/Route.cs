using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Animation
{
    public class Route : RuntimeBehaviour
    {
        public UnityEvent onComplete;

        private Transform target;

        private int index, count;

        private float speed;

        private float step;

        private Status status;

        private Vector3 position;

        private readonly List<Segment> segments = new List<Segment>();

        protected override void OnUpdate(float delta)
        {
            switch (status)
            {
                case Status.Run:
                    {
                        step += speed * Time.deltaTime;

                        for (int i = index; i < count; i++)
                        {
                            if (segments[i].Exist(step))
                            {
                                position = segments[i].Position(step);
                                index = i;
                                break;
                            }
                        }
                        if (step >= segments[count - 1].max)
                        {
                            status = Status.Complete;
                        }
                        Translation();
                    }
                    break;
                case Status.Complete:
                    {
                        Complete();
                    }
                    break;
            }
        }

        private void Translation()
        {
            target.position = position;
        }

        private void Complete()
        {
            status = Status.Idle;

            onComplete?.Invoke();
        }

        public void Startup(Transform target, List<Vector3> routes, float speed)
        {
            if (target == null || routes == null || routes.Count == 0)
            {
                return;
            }

            this.target = target;

            this.speed = speed > 0 ? speed : 1;

            segments.Clear();

            count = routes.Count;

            for (int i = 1; i < count; i++)
            {
                segments.Add(new Segment(routes[i - 1], routes[i])
                {
                    index = i,
                });
                segments[i - 1].min = i > 1 ? segments[i - 2].max : 0;
                segments[i - 1].max = segments[i - 1].min + segments[i - 1].length;
            }

            index = 0;

            count = segments.Count;

            step = 0;

            status = Status.Run;
        }

        class Segment
        {
            public int index;

            public float min, max;

            public float length;

            public Vector3 origination, destination;

            public Segment(Vector3 start, Vector3 end)
            {
                origination = start;

                destination = end;

                length = Vector3.Distance(origination, destination);
            }

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

        enum Status
        {
            Idle,
            Ready,
            Run,
            Complete,
        }
    }
}