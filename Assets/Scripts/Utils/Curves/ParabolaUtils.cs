using UnityEngine;

namespace Game
{
    public static class ParabolaUtils
    {
        /// <summary>
        /// 已知抛物线上一点，求角度(0, 45)
        /// </summary>
        public static float Angle(Vector3 start, Vector3 position, float speed)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(position.x - start.x, 2) + Mathf.Pow(position.z - start.z, 2));

            float height = position.y - start.y;

            float max = Distance(speed, 45);

            if (max < distance) return -1;

            max = Height(speed, 45);

            if (max < height) return -1;

            float p = height / distance;

            float k = p + distance * Parabola.G / (speed * speed);

            float v = p * p - k * k + 1;

            if (v < 0) return -1;

            float x = (-k * p + Mathf.Sqrt(v)) / (p * p + 1);

            float y = (-k * p - Mathf.Sqrt(v)) / (p * p + 1);

            float v1 = Mathf.Acos(x) * 0.5f * Mathf.Rad2Deg;

            float v2 = Mathf.Acos(y) * 0.5f * Mathf.Rad2Deg;

            if (v1 > 0 && v1 <= 45)
            {
                return v1;
            }
            else if (v2 > 0 && v2 <= 45)
            {
                return v2;
            }
            return -1;
        }
        /// <summary>
        /// 通过角度，求抛物线距离
        /// </summary>
        public static float Distance(float speed, float angle)
        {
            float cos = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

            float sin = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

            return sin / Parabola.HG * cos;
        }
        /// <summary>
        /// 通过角度，求抛物线最大高度
        /// </summary>
        public static float Height(float speed, float angle)
        {
            float sin = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

            float time = sin / Parabola.HG * 0.5f;

            return sin * time - Parabola.HG * Mathf.Pow(time, 2);
        }
        /// <summary>
        /// 通过角度+距离，求另一点
        /// </summary>
        public static Vector3 Position(Vector3 origin, float angle, float distance)
        {
            return new Vector3
            {
                x = origin.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = origin.y,
                z = origin.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad),
            };
        }
        /// <summary>
        /// 通过角度，求抛物线落点位置
        /// </summary>
        public static Vector3 Position(Vector3 origin, float horiontal, float vertical, float speed)
        {
            float cos = speed * Mathf.Cos(vertical * Mathf.Deg2Rad);

            float sin = speed * Mathf.Sin(vertical * Mathf.Deg2Rad);

            float distance = sin * cos / Parabola.HG;

            return new Vector3
            {
                x = origin.x + distance * Mathf.Cos(horiontal * Mathf.Deg2Rad),
                y = origin.y,
                z = origin.z + distance * Mathf.Sin(horiontal * Mathf.Deg2Rad),
            };
        }
    }
}