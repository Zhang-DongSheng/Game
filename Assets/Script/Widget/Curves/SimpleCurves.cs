using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SimpleCurves
    {
        private readonly float G = 9.8f;

        private readonly Dictionary<int, float> mapping = new Dictionary<int, float>();

        private readonly List<Vector3> curves = new List<Vector3>();

        public List<Vector3> FetchCurves(Vector2 start, Vector2 end, float speed, int step = 3, bool cast = false)
        {
            float distance = Vector3.Distance(start, end);

            if (mapping.TryGetValue((int)distance, out float angle))
            {
                angle = cast ? 90 - angle : angle;
            }
            else
            {
                angle = Angle(distance, speed);
            }

            if (float.IsNaN(angle))
            {
                return null;
            }
            else if (angle == -1)
            {
                Debug.LogError("³¬³ö×î´ó¾àÀë£¡");
                return null;
            }
            else
            {
                curves.Clear();

                float progress, count = step - 1;

                float cos = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

                float sin = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

                for (int i = 0; i < step; i++)
                {
                    progress = i / count;

                    Vector3 point = Vector3.Lerp(start, end, progress);

                    point.y += Height(Vector3.Distance(start, point), cos, sin);

                    curves.Add(point);
                }
                return curves;
            }
        }

        private float Angle(float distance, float speed)
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

            if (!mapping.ContainsKey((int)distance))
            {
                mapping.Add((int)distance, angle);
            }
            return angle;
        }

        private float Height(float x, float cos, float sin)
        {
            float time = x / cos;

            float height = sin * time - 0.5f * G * Mathf.Pow(time, 2);

            return height;
        }
    }
}