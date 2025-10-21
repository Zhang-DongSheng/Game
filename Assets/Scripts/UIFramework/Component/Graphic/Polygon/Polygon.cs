using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 多边形
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    public class Polygon : MaskableGraphic
    {
        [SerializeField] private Sprite sprite;

        [SerializeField] private float radius = 100;

        [SerializeField] private int count = 3;

        [SerializeField] private List<Vector2> points;

        private Vector2 space = new Vector2();

        protected override void Awake()
        {
            if (points.Count != count)
            {
                Rebuilder();
            }
            base.Awake();
        }

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            if (!isActiveAndEnabled) return;

            helper.Clear();

            space.x = rectTransform.rect.width;

            space.y = rectTransform.rect.height;

            if (points != null && points.Count > 2)
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
            else
            {
                Debuger.LogWarning(Author.UI, "The Polygon Points count must be more than 3!");
            }
        }

        private void Rebuilder()
        {
            count = Mathf.Max(3, count);

            points.Clear();

            float angle = 360f / count;

            for (int i = 0; i < count; i++)
            {
                points.Add(new Vector2()
                {
                    x = Mathf.Sin(i * angle * Mathf.Deg2Rad) * radius,
                    y = Mathf.Cos(i * angle * Mathf.Deg2Rad) * radius,
                });
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

        public override Texture mainTexture => sprite == null ? s_WhiteTexture : sprite.texture;
    }
}