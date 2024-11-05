using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Â·¾¶
    /// </summary>
    public class Route
    {
        private int count;

        private float distance;

        private readonly List<RoutePoint> _routes = new List<RoutePoint>();

        public void Execute(List<Vector3> points)
        {
            _routes.Clear();

            distance = 0;

            count = points.Count;

            for (int i = 1; i < count; i++)
            {
                int index = i - 1;

                _routes.Add(new RoutePoint(points[index], points[i], distance));

                distance += _routes[index].length;
            }
        }

        public void Lerp(float distance, ref Vector3 position, ref Quaternion rotation)
        {
            count = _routes.Count;

            for (int i = 0; i < count; i++)
            {
                if (_routes[i].min <= distance &&
                    _routes[i].max >= distance)
                {
                    _routes[i].Lerp(distance, ref position, ref rotation);
                    break;
                }
            }
        }

        public float Distance { get { return distance; } }

        class RoutePoint
        {
            public float min, max;

            public float length;

            public Vector3 start, end;

            public Quaternion rotation;

            public RoutePoint(Vector3 start, Vector3 end, float distance)
            {
                this.start = start;

                this.end = end;

                length = Vector3.Distance(start, end);

                min = distance;

                max = distance + length;

                rotation = Quaternion.LookRotation(end - start, Vector3.forward);
            }

            public void Lerp(float distance, ref Vector3 position, ref Quaternion rotation)
            {
                if (distance > min)
                { 
                    position = Vector3.Lerp(start, end, (distance - min) / length);

                    rotation = this.rotation;
                }
                position = start;
            }
        }
    }
}