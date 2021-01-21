using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GraphicColorFade : MaskableGraphic
    {
        [SerializeField] private Axis axis;

        [SerializeField] private Color origin = Color.white;

        [SerializeField] private Color destination = Color.white;

        private new Color color;

        private float width, height;

        private Vector2 topLeft, topRight, bottomLeft, bottomRight;

        private readonly List<UIVertex> m_vertexs = new List<UIVertex>();

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            if (!IsActive()) return;

            helper.Clear(); m_vertexs.Clear();

            width = GetComponent<RectTransform>().rect.width / 2f;

            height = GetComponent<RectTransform>().rect.height / 2f;

            topLeft = new Vector2(width * -1, height);

            topRight = new Vector2(width, height);

            bottomLeft = new Vector2(width * -1, height * -1);

            bottomRight = new Vector2(width, height * -1);

            m_vertexs.Add(AddUIVertex(topLeft, Location.TopLeft));

            m_vertexs.Add(AddUIVertex(topRight, Location.TopRight));

            m_vertexs.Add(AddUIVertex(bottomLeft, Location.BottomLeft));

            m_vertexs.Add(AddUIVertex(bottomRight, Location.BottomRight));

            m_vertexs.Add(AddUIVertex(bottomLeft, Location.BottomLeft));

            m_vertexs.Add(AddUIVertex(topRight, Location.TopRight));

            helper.AddUIVertexTriangleStream(m_vertexs);
        }

        private UIVertex AddUIVertex(Vector2 position, Location location)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    switch (location)
                    {
                        case Location.TopLeft:
                        case Location.BottomLeft:
                            color = origin * base.color;
                            break;
                        default:
                            color = destination * base.color;
                            break;
                    }
                    break;
                case Axis.Vertical:
                    switch (location)
                    {
                        case Location.TopLeft:
                        case Location.TopRight:
                            color = origin * base.color;
                            break;
                        default:
                            color = destination * base.color;
                            break;
                    }
                    break;
                default:
                    switch (location)
                    {
                        case Location.TopLeft:
                            color = origin * base.color;
                            break;
                        case Location.BottomRight:
                            color = destination * base.color;
                            break;
                        default:
                            color = Color.Lerp(origin, destination, 0.5f);
                            break;
                    }
                    break;
            }

            return new UIVertex()
            {
                position = position,
                color = color,
            };
        }

        enum Location
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        enum Axis
        {
            Horizontal,
            Vertical,
            Slant,
        }
    }
}