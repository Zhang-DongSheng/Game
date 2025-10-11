using UnityEngine;

namespace Game.World
{
    public static class EntityUtils
    {
        /// <summary>
        /// 定向
        /// </summary>
        public static Quaternion Orientation(Vector3 position, float radius)
        {
            var points = PickPoints(position, radius);

            var normal = Normal(points[0], points[1], points[2]);

            return Quaternion.LookRotation(Vector3.forward, -normal);
        }
        /// <summary>
        /// 取点
        /// </summary>
        public static Vector3[] PickPoints(Vector3 position, float radius)
        {
            var count = 3;

            var angle = 360f / count;

            var points = new Vector3[count];

            var layer = 1 << LayerMask.NameToLayer("Default");

            position.y += 10f;

            for (int i = 0; i < count; i++)
            {
                points[i] = position + RelativePosition(i * angle, radius);

                if (Physics.Raycast(points[i], Vector3.down, out var hit, 15,layer))
                {
                    points[i] = hit.point;
                }
                else
                {
                    points[i].y -= 10f;
                }
            }
            return points;
        }
        /// <summary>
        /// 相对位置
        /// </summary>
        public static Vector3 RelativePosition(float angle, float distance)
        {
            return new Vector3()
            {
                x = distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = 0,
                z = distance * Mathf.Sin(angle * Mathf.Deg2Rad)
            };
        }
        /// <summary>
        /// 法线
        /// </summary>
        public static Vector3 Normal(Vector3 a, Vector3 b, Vector3 c)
        {
            var na = (b.y - a.y) * (c.z - a.z) - (b.z - a.z) * (c.y - a.y);

            var nb = (b.z - a.z) * (c.x - a.x) - (b.x - a.x) * (c.z - a.z);

            var nc = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);

            return new Vector3(na, nb, nc);
        }
    }
}