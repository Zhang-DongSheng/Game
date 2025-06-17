using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Parabola
    {
        public const float G = 9.8f;

        public const float HG = G * 0.5f;

        private readonly List<Vector3> curves = new List<Vector3>();

        private readonly Dictionary<int, float> mapping = new Dictionary<int, float>();
        /// <summary>
        /// 求抛物线
        /// </summary>
        public List<Vector3> FetchCurves(Vector3 start, Vector3 end, float speed, out float angle, int step = 3, bool cast = false)
        {
            float distance = Vector3.Distance(start, end);

            curves.Clear();

            angle = Mapping(speed, distance);

            angle = cast ? 90 - angle : angle;

            if (float.IsNaN(angle))
            {
                return curves;
            }
            else if (angle == -1)
            {
                return curves;
            }
            else
            {
                float progress, count = step - 1;

                float cos = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

                float sin = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

                float time;

                for (int i = 0; i < step; i++)
                {
                    progress = i / count;

                    Vector3 point = Vector3.Lerp(start, end, progress);

                    time = Vector3.Distance(start, point) / cos;

                    point.y += sin * time - HG * Mathf.Pow(time, 2);

                    curves.Add(point);
                }
                return curves;
            }
        }
        /// <summary>
        /// 求抛物线
        /// </summary>
        public List<Vector3> FetchCurves(Vector3 start, float speed, Vector2 angle, int step = 3)
        {
            curves.Clear();

            if (float.IsNaN(angle.y))
            {
                return curves;
            }
            else if (angle.y == -1)
            {
                return curves;
            }
            else
            {
                float count = step - 1;

                float cos = speed * Mathf.Cos(angle.y * Mathf.Deg2Rad);

                float sin = speed * Mathf.Sin(angle.y * Mathf.Deg2Rad);

                float distance = sin / HG * cos;

                float time;

                for (int i = 0; i < step; i++)
                {
                    float width = Mathf.Lerp(0, distance, i / count);

                    time = width / cos;

                    float height = sin * time - HG * Mathf.Pow(time, 2);

                    Vector3 point = new Vector3
                    {
                        x = start.x + width * Mathf.Cos(angle.x * Mathf.Deg2Rad),
                        y = start.y + height,
                        z = start.z + width * Mathf.Sin(angle.x * Mathf.Deg2Rad),
                    };
                    curves.Add(point);
                }
                return curves;
            }
        }
        /// <summary>
        /// 通过距离求抛物线角度
        /// </summary>
        public float Angle(float speed, float distance)
        {
            float angle;

            float value = Mathf.Pow(speed, 4) - Mathf.Pow(G, 2) * Mathf.Pow(distance, 2);

            if (value == 0)
            {
                angle = 45f;
            }
            else if (value > 0)
            {
                float time = 2 * (Mathf.Pow(speed, 2) - Mathf.Sqrt(value)) / Mathf.Pow(G, 2);

                time = Mathf.Sqrt(time);

                float radian = Mathf.Acos(distance / (speed * time));

                angle = Mathf.Rad2Deg * radian;
            }
            else
            {
                angle = -1;
            }
            return angle;
        }
        /// <summary>
        /// 映射
        /// </summary>
        public float Mapping(float speed, float distance)
        {
            if (mapping.TryGetValue((int)distance, out float angle))
            {
                return angle;
            }

            angle = Angle(speed, distance);

            mapping.Add((int)distance, angle);

            return angle;
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            mapping.Clear();
        }
    }
}