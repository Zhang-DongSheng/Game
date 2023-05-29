using Game.Attribute;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class TestCurves : MonoBehaviour
    {
        [SerializeField] private Vector2 angle;

        [SerializeField] private float speed = 36;

        [SerializeField] private int step = 10;

        [SerializeField] private Vector3 start;

        [SerializeField] private Vector3 end;

        [SerializeField] private bool kind;
        [Button("OnClick")]
        [SerializeField] private int index;

        private readonly Parabola curves = new Parabola();

        private readonly List<Vector3> nodes = new List<Vector3>();

        private void OnValidate()
        {
            nodes.Clear();

            if (kind)
            {
                nodes.AddRange(curves.FetchCurves(start, end, speed, out float _angle, step, false));

                Debuger.LogError(Author.Test, "当前角度" + _angle);
            }
            else
            {
                nodes.AddRange(curves.FetchCurves(start, speed, angle, step));

                end = nodes[nodes.Count - 1];

                Debuger.LogError(Author.Test, "当前角度" + angle);
            }
            ///*
            for (int i = 0, count = nodes.Count; i < count; i++)
            {
                Debuger.Log(Author.Test, $"第{i}点 x:{nodes[i].x}, y:{nodes[i].y}");
            }
            //*/
        }

        private void OnDrawGizmos()
        {
            if (nodes == null || nodes.Count == 0) return;

            Gizmos.color = Color.green;

            for (int i = 0; i < nodes.Count; i++)
            {
                Gizmos.DrawSphere(nodes[i], 0.8f);
            }
            Gizmos.color = Color.yellow;

            for (int i = 1; i < nodes.Count; i++)
            {
                Gizmos.DrawLine(nodes[i - 1], nodes[i]);
            }
        }

        private void OnClick(int index)
        {
            Vector3 position = nodes[index];

            float angle = ParabolaUtils.Angle(start, position, speed);

            Debuger.LogError(Author.Test, $"目标位置 x:{position.x}, y:{position.y}");

            Debuger.LogError(Author.Test, "目标角度" + angle);

            var vector = position - start;

            angle = vector.Angle();

            Debuger.LogError(Author.Test, "角度" + angle);
        }
    }
}
