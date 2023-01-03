using Game.UI;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 图形混色
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasRenderer))]
    public class GraphicColorMixed : MaskableGraphic
    {
        [SerializeField] private Direction direction;

        [SerializeField] private Color fade = Color.white;

        private Color pigment;

        private Vector2 topLeft, topRight, bottomLeft, bottomRight;

        private readonly List<UIVertex> m_vertexs = new List<UIVertex>();

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            if (!IsActive()) return;

            helper.Clear(); m_vertexs.Clear();

            float width = GetComponent<RectTransform>().rect.width / 2f;

            float height = GetComponent<RectTransform>().rect.height / 2f;

            topLeft = new Vector2(width * -1, height);

            topRight = new Vector2(width, height);

            bottomLeft = new Vector2(width * -1, height * -1);

            bottomRight = new Vector2(width, height * -1);

            m_vertexs.Add(AddUIVertex(topLeft, Corner.TopLeft));

            m_vertexs.Add(AddUIVertex(topRight, Corner.TopRight));

            m_vertexs.Add(AddUIVertex(bottomLeft, Corner.LowerLeft));

            m_vertexs.Add(AddUIVertex(bottomRight, Corner.LowerRight));

            m_vertexs.Add(AddUIVertex(bottomLeft, Corner.LowerLeft));

            m_vertexs.Add(AddUIVertex(topRight, Corner.TopRight));

            helper.AddUIVertexTriangleStream(m_vertexs);
        }

        private UIVertex AddUIVertex(Vector2 position, Corner corner)
        {
            switch (direction)
            {
                case Direction.Horizontal:
                    switch (corner)
                    {
                        case Corner.TopLeft:
                        case Corner.LowerLeft:
                            pigment = base.color;
                            break;
                        default:
                            pigment = fade;
                            break;
                    }
                    break;
                case Direction.Vertical:
                    switch (corner)
                    {
                        case Corner.TopLeft:
                        case Corner.TopRight:
                            pigment = base.color;
                            break;
                        default:
                            pigment = fade;
                            break;
                    }
                    break;
                case Direction.Custom:
                    switch (corner)
                    {
                        case Corner.TopLeft:
                            pigment = base.color;
                            break;
                        case Corner.LowerRight:
                            pigment = fade;
                            break;
                        default:
                            pigment = Color.Lerp(base.color, fade, 0.5f);
                            break;
                    }
                    break;
                default:
                    pigment = base.color;
                    break;
            }

            return new UIVertex()
            {
                position = position,
                color = pigment,
            };
        }
    }
}