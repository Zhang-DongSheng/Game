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
            Spline2,
        }

        [SerializeField] private CurvesTpye type;

        [SerializeField] private List<Vector3> points;

        [SerializeField, Range(1, 25)] private int ratio;

        private readonly List<Vector3> nodes = new List<Vector3>();

        private readonly SplineCurves2 spline = new SplineCurves2();

        private void OnValidate()
        {
            if (points == null || points.Count < 3) return;

            nodes.Clear();

            switch (type)
            {
                case CurvesTpye.Bezier:
                    nodes.AddRange(BezierCurves.FetchCurves(ratio, points.ToArray()));
                    break;
                case CurvesTpye.Spline:
                    nodes.AddRange(SplineCurves.FetchCurves(ratio, points.ToArray()));
                    break;
                case CurvesTpye.Spline2:
                    nodes.AddRange(spline.FetchCurves(ratio, points.ToArray()));
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (points == null) return;

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
        }
    }
}