using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 贝塞尔曲线
/// </summary>
public class StudyBezier : MonoBehaviour
{
    [SerializeField] private List<Vector3> points;

    [SerializeField, Range(1, 10)] private int ratio;

    private int count;

    private readonly List<Vector3> nodes = new List<Vector3>();

    private void OnDrawGizmos()
    {
        if (points.Count < 3) return;

        Gizmos.color = Color.green;

        nodes.Clear();

        count = points.Count * ratio;

        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(points[i], 1);

            for (int j = 0; j < ratio; j++)
            {
                nodes.Add(Bezier((float)(i * ratio + j) / count, points));
            }
        }

        Gizmos.color = Color.yellow;

        for (int i = 1; i < nodes.Count; i++)
        {
            Gizmos.DrawLine(nodes[i - 1], nodes[i]);
        }
    }

    private Vector3 Bezier(float t, Vector3 p0, Vector3 p1)
    {
        return Vector3.Lerp(p0, p1, t);
    }

    private Vector3 Bezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 p0p1 = Vector3.Lerp(p0, p1, t);

        Vector3 p1p2 = Vector3.Lerp(p1, p2, t);

        return Vector3.Lerp(p0p1, p1p2, t);
    }

    private Vector3 Bezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 p0p1 = Vector3.Lerp(p0, p1, t);

        Vector3 p1p2 = Vector3.Lerp(p1, p2, t);

        Vector3 p2p3 = Vector3.Lerp(p2, p3, t);

        Vector3 p0p1p2 = Vector3.Lerp(p0p1, p1p2, t);

        Vector3 p1p2p3 = Vector3.Lerp(p1p2, p2p3, t);

        return Vector3.Lerp(p0p1p2, p1p2p3, t);
    }

    private Vector3 Bezier(float t, List<Vector3> p)
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