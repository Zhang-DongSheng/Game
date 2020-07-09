using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicColorFade : BaseMeshEffect
    {
        [SerializeField] private Axis axis;

        [SerializeField] private Color32 origin = Color.white;

        [SerializeField] private Color32 destination = Color.white;

        private Vector2 horizontal = new Vector2();

        private Vector2 vertical = new Vector2();

        private float width;

        private float height;

        private readonly List<UIVertex> m_vertexs = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper helper)
        {
            if (!IsActive()) return;

            int count = helper.currentVertCount;

            if (count == 0) return;

            m_vertexs.Clear();

            for (var i = 0; i < count; i++)
            {
                var vertex = new UIVertex();

                helper.PopulateUIVertex(ref vertex, i);

                m_vertexs.Add(vertex);
            }

            switch (axis)
            {
                case Axis.Slant:
                    Compute(Axis.Horizontal);
                    Compute(Axis.Vertical);
                    break;
                default:
                    Compute(axis);
                    break;
            }

            for (int i = 0; i < count; i++)
            {
                var vertex = m_vertexs[i];

                Color color = Color32.Lerp(origin, destination, Progress(vertex.position));

                vertex.color = color;

                helper.SetUIVertex(vertex, i);
            }
        }

        /// <summary>
        /// 获取大小及长宽
        /// </summary>
        /// <param name="axis">方向</param>
        private void Compute(Axis axis)
        {
            float position;

            switch (axis)
            {
                case Axis.Horizontal:
                    horizontal.x = m_vertexs[0].position.x;
                    horizontal.y = m_vertexs[0].position.x;

                    for (var i = 1; i < m_vertexs.Count; i++)
                    {
                        position = m_vertexs[i].position.x;

                        if (position > horizontal.x)
                        {
                            horizontal.x = position;
                        }
                        else if (position < horizontal.y)
                        {
                            horizontal.y = position;
                        }
                    }
                    width = horizontal.x - horizontal.y;
                    break;
                case Axis.Vertical:
                    vertical.x = m_vertexs[0].position.y;
                    vertical.y = m_vertexs[0].position.y;

                    for (var i = 1; i < m_vertexs.Count; i++)
                    {
                        position = m_vertexs[i].position.y;

                        if (position > vertical.x)
                        {
                            vertical.x = position;
                        }
                        else if (position < vertical.y)
                        {
                            vertical.y = position;
                        }
                    }
                    height = vertical.x - vertical.y;
                    break;
            }
        }

        /// <summary>
        /// 计算当前位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns></returns>
        private float Progress(Vector2 position)
        {
            float progress = 0;

            switch (axis)
            {
                case Axis.Horizontal:
                    progress = (position.x - horizontal.y) / width;
                    break;
                case Axis.Vertical:
                    progress = (position.y - vertical.y) / height;
                    break;
                case Axis.Slant:
                    progress = ((position.x - horizontal.y) / width + (position.y - vertical.y) / height) / 2f;
                    break;
                default:
                    break;
            }

            return progress;
        }

        enum Axis
        {
            Horizontal,
            Vertical,
            Slant,
        }
    }
}