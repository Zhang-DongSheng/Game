using Game;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class TestDraw : MonoBehaviour
    {
        enum CurvesTpye
        { 
            Bezier,
            Spline,
        }

        [SerializeField] private CurvesTpye type;

        [SerializeField] private List<Vector3> points;

        [SerializeField, Range(1, 10)] private int ratio;

        private int count;

        private readonly List<Vector3> nodes = new List<Vector3>();

        private readonly List<SplineCurves> scs = new List<SplineCurves>();

        private void OnValidate()
        {
            Compute();
        }

        private void Compute()
        {
            scs.Clear();

            if (points != null && points.Count > 1)
            {
                SplineCurves spline, pre;

                for (int i = 0; i < points.Count; i++)
                {
                    if (scs.Count == 0)
                    {
                        spline = new SplineCurves();
                        spline.AddJoint(null, points[i]);
                        scs.Add(spline);
                    }
                    else
                    {
                        spline = new SplineCurves();
                        pre = scs[scs.Count - 1];
                        spline.AddJoint(pre, points[i]);
                        scs.Add(spline);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            switch (type)
            {
                case CurvesTpye.Bezier:
                    if (points.Count < 3) return;

                    Gizmos.color = Color.green;

                    nodes.Clear();

                    count = points.Count * ratio;

                    for (int i = 0; i < points.Count; i++)
                    {
                        Gizmos.DrawSphere(points[i], 1);

                        for (int j = 0; j < ratio; j++)
                        {
                            nodes.Add(BezierCurves.Bezier((float)(i * ratio + j) / count, points));
                        }
                    }

                    Gizmos.color = Color.yellow;

                    for (int i = 1; i < nodes.Count; i++)
                    {
                        Gizmos.DrawLine(nodes[i - 1], nodes[i]);
                    }
                    break;
                case CurvesTpye.Spline:
                    if (points == null || points.Count < 3) return;

                    Gizmos.color = Color.green;

                    for (int i = 0; i < points.Count; i++)
                    {
                        Gizmos.DrawSphere(points[i], 1);
                    }

                    Gizmos.color = Color.yellow;

                    for (int i = 1; i < scs.Count; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        scs[i].Draw();
                    }
                    break;
            }
        }
    }
}