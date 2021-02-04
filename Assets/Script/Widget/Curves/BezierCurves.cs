using System.Collections.Generic;
using UnityEngine;

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

        public static Vector3 Bezier(float t, params Vector3[] points)
        {
            if (points.Length <= 1) return points[0];

            Vector3[] newP = new Vector3[points.Length - 1];

            for (int i = 1; i < points.Length; i++)
            {
                newP[i - 1] = Vector3.Lerp(points[i - 1], points[i], t);
            }

            return Bezier(t, newP);
        }

        public static List<Vector3> FetchCurves(int ratio, params Vector3[] points)
        {
            List<Vector3> curves = new List<Vector3>();

            int count = points.Length * ratio;

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < ratio; j++)
                {
                    curves.Add(Bezier((float)(i * ratio + j) / count, points));
                }
            }

            return curves;
        }
    }
}