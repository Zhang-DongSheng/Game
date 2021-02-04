using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SplineCurves2
    {
        private static Vector3 a, b, c, d;

        public static List<Vector3> FetchCurves(int ratio, params Vector3[] points)
        {
            List<Vector3> curves = new List<Vector3>();

            Vector3[] nodes = Generator(points);

            Vector3 current;

            float step;

            int count = nodes.Length * ratio;

            int end = nodes.Length - 3;

            for (int i = 0; i <= count; i++)
            {
                step = i / (float)count;

                current = Lerp(nodes, end, step);

                curves.Add(current);
            }

            return curves;
        }

        private static Vector3[] Generator(params Vector3[] points)
        {
            int length = points.Length + 2;

            Vector3[] nodes = new Vector3[length];

            Array.Copy(points, 0, nodes, 1, points.Length);

            nodes[0] = nodes[1] + (nodes[1] - nodes[2]);

            nodes[length - 1] = nodes[length - 2] + (nodes[length - 2] - nodes[length - 3]);

            if (nodes[1] == nodes[length - 2])
            {
                Vector3[] spline = new Vector3[length];

                Array.Copy(nodes, spline, length);

                spline[0] = spline[length - 3];

                spline[length - 1] = spline[2];

                nodes = new Vector3[length];

                Array.Copy(spline, nodes, length);
            }
            return nodes;
        }

        private static Vector3 Lerp(Vector3[] points, int end, float t)
        {
            int index = Mathf.Min(Mathf.FloorToInt(t * (float)end), end - 1);

            float u = t * (float)end - (float)index;

            a = points[index];
            b = points[index + 1];
            c = points[index + 2];
            d = points[index + 3];

            return 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u)
                + (2f * a - 5f * b + 4f * c - d) * (u * u)
                + (-a + c) * u
                + 2f * b
            );
        }
    }
}