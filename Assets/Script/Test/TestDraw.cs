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

        private void OnValidate()
        {
            
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

                    nodes.Clear();

                    nodes.AddRange(SplineCurves.FetchPoints(points.ToArray()));

                    Gizmos.color = Color.green;

                    for (int i = 0; i < points.Count; i++)
                    {
                        Gizmos.DrawSphere(points[i], 1);
                    }

                    Gizmos.color = Color.yellow;

                    for (int i = 1; i < nodes.Count; i++)
                    {
                        Gizmos.DrawLine(nodes[i - 1], nodes[i]);
                    }
                    break;
            }
        }
    }
}