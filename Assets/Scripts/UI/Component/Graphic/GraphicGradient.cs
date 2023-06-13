using Game.UI;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// Í¼ÐÎ½¥±ä
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class GraphicGradient : BaseMeshEffect
    {
        [SerializeField] private Direction direction;

        [SerializeField] private Color from = Color.white;

        [SerializeField] private Color to = Color.white;

        [SerializeField] private bool local;

        [SerializeField, Range(0, 360)] private float rotation;

        private float top, bottom, height;

        private UIVertex vertex = new UIVertex();

        private readonly List<UIVertex> m_vertexs = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper helper)
        {
            if (!isActiveAndEnabled)
                return;
            int count = helper.currentVertCount;
            if (count == 0)
                return;
            helper.GetUIVertexStream(m_vertexs);
            if (local)
            {
                for (int i = 0; i < count; i++)
                {
                    helper.PopulateUIVertex(ref vertex, i);
                    switch (direction)
                    {
                        case Direction.Vertical:
                            vertex.color *= (i % 4 == 0 || (i - 3) % 4 == 0) ? from : to;
                            break;
                        case Direction.Horizontal:
                            vertex.color *= (i % 4 == 0 || (i - 1) % 4 == 0) ? from : to;
                            break;
                        case Direction.Custom:
                            {
                                float angle = Vector2.SignedAngle(vertex.position, Vector2.up);

                                if (angle > 0)
                                {
                                    angle %= 360;
                                }
                                else
                                {
                                    while (angle < 0)
                                    {
                                        angle += 360;
                                    }
                                }
                                angle = Mathf.Abs(angle - rotation);

                                if (angle > 180)
                                {
                                    angle = 360 - angle;
                                }
                                vertex.color *= Color.Lerp(from, to, angle / 180f);
                            }
                            break;
                    }
                    helper.SetUIVertex(vertex, i);
                }
            }
            else
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        {
                            top = m_vertexs[0].position.x;
                            bottom = m_vertexs[count - 1].position.x;
                            height = top - bottom;

                            for (int i = 0; i < count; i++)
                            {
                                helper.PopulateUIVertex(ref vertex, i);
                                vertex.color *= Color.Lerp(to, from, (vertex.position.x - bottom) / height);
                                helper.SetUIVertex(vertex, i);
                            }
                        }
                        break;
                    case Direction.Vertical:
                        {
                            top = m_vertexs[0].position.y;
                            bottom = m_vertexs[count - 1].position.y;
                            height = top - bottom;

                            for (int i = 0; i < count; i++)
                            {
                                helper.PopulateUIVertex(ref vertex, i);
                                vertex.color *= Color.Lerp(to, from, (vertex.position.y - bottom) / height);
                                helper.SetUIVertex(vertex, i);
                            }
                        }
                        break;
                    default: goto case Direction.Horizontal;
                }
            }
        }
    }
}