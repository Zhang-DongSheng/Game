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

        [SerializeField, Range(0, 360)] private float rotation;

        [SerializeField] private Color fade = Color.white;

        private readonly Vector2[] points = new Vector2[4];

        private readonly List<UIVertex> m_vertexs = new List<UIVertex>();

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            if (!IsActive()) return;

            helper.Clear(); m_vertexs.Clear();

            float width = GetComponent<RectTransform>().rect.width / 2f;

            float height = GetComponent<RectTransform>().rect.height / 2f;

            points[0] = new Vector2(width * -1, height);

            points[1] = new Vector2(width, height);

            points[2] = new Vector2(width * -1, height * -1);

            points[3] = new Vector2(width, height * -1);

            m_vertexs.Add(NewVertex(points[0]));

            m_vertexs.Add(NewVertex(points[1]));

            m_vertexs.Add(NewVertex(points[2]));

            m_vertexs.Add(NewVertex(points[1]));

            m_vertexs.Add(NewVertex(points[2]));

            m_vertexs.Add(NewVertex(points[3]));

            helper.AddUIVertexTriangleStream(m_vertexs);
        }

        private UIVertex NewVertex(Vector2 position)
        {
            Color GetColor(Vector2 position)
            {
                switch (direction)
                {
                    case Direction.Vertical:
                        return position.y > 0 ? color : fade;
                    case Direction.Horizontal:
                        return position.x < 0 ? color : fade;
                    case Direction.Custom:
                        {
                            float angle = Vector2.SignedAngle(position, Vector2.up);

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
                            return Color.Lerp(color, fade, angle / 180f);
                        }
                }
                return color;
            }
            return new UIVertex()
            {
                position = position,
                color = GetColor(position),
            };
        }
    }
}