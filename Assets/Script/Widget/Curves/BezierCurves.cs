using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    public static class BezierCurves
    {
        public static Vector3 Bezier(float t, Vector3 p0, Vector3 p1)
        {
            return Vector3.Lerp(p0, p1, t);
        }

        public static Vector3 Bezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 p0p1 = Vector3.Lerp(p0, p1, t);

            Vector3 p1p2 = Vector3.Lerp(p1, p2, t);

            return Vector3.Lerp(p0p1, p1p2, t);
        }

        public static Vector3 Bezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 p0p1 = Vector3.Lerp(p0, p1, t);

            Vector3 p1p2 = Vector3.Lerp(p1, p2, t);

            Vector3 p2p3 = Vector3.Lerp(p2, p3, t);

            Vector3 p0p1p2 = Vector3.Lerp(p0p1, p1p2, t);

            Vector3 p1p2p3 = Vector3.Lerp(p1p2, p2p3, t);

            return Vector3.Lerp(p0p1p2, p1p2p3, t);
        }

        public static Vector3 Bezier(float t, List<Vector3> p)
        {
            if (p.Count <= 1) return p[0];

            List<Vector3> newP = new List<Vector3>();

            for (int i = 1; i < p.Count; i++)
            {
                newP.Add(Vector3.Lerp(p[i - 1], p[i], t));
            }

            return Bezier(t, newP);
        }
    }
}