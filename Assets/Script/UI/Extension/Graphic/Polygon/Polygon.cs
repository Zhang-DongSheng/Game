using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 自定义多边形
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class Polygon : MaskableGraphic
    {
        [SerializeField] private Texture2D texture;

        [SerializeField]
        private List<Vector2> points = new List<Vector2>()
        {
            new Vector2(50, 50), new Vector2(50, -50), new Vector2(-50, -50), new Vector2(-50, 50)
        };

        private Vector2 space = new Vector2();

        public override Texture mainTexture => texture == null ? s_WhiteTexture : texture;

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();

            space.x = rectTransform.rect.width;

            space.y = rectTransform.rect.height;

            if (points != null && points.Count > 0)
            {
                int count = points.Count;

                //设置坐标点
                foreach (var point in points)
                {
                    helper.AddVert(GetUIVertex(point));
                }

                //自定义三角形
                for (int i = 1; i < count - 1; i++)
                {
                    helper.AddTriangle(i, 0, i + 1);
                }
            }
        }

        private UIVertex GetUIVertex(Vector3 position)
        {
            return new UIVertex()
            {
                position = position,
                color = color,
                uv0 = new Vector2(position.x / space.x + 0.5f, position.y / space.y + 0.5f),
            };
        }
    }
}