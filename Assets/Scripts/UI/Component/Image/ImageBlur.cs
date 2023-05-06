using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.UI
{
    /// <summary>
    /// 高斯模糊，未完成-UGUI好像只有顶点处理，未发现片元处理
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class ImageBlur : MaskableGraphic
    {
        [SerializeField] private float blur;

        [SerializeField] private Sprite sprite;

        private Vector2 space = new Vector2(0, 0);

        private readonly Vector4[] gauss = new Vector4[7]
        {
            new Vector4(0.0205f, 0.0205f, 0.0205f, 0),
            new Vector4(0.0855f, 0.0855f, 0.0855f, 0),
            new Vector4(0.232f, 0.232f, 0.232f, 0),
            new Vector4(0.324f, 0.324f, 0.324f, 1),
            new Vector4(0.232f, 0.232f, 0.232f, 0),
            new Vector4(0.0855f, 0.0855f, 0.0855f, 0),
            new Vector4(0.0205f, 0.0205f, 0.0205f, 0)
        };
        private readonly List<Vector2> points = new List<Vector2>(4);

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            if (!isActiveAndEnabled) return;

            helper.Clear();

            space.x = rectTransform.rect.width;

            space.y = rectTransform.rect.height;

            float x = space.x * 0.5f;

            float y = space.y * 0.5f;

            points.Clear();

            points.Add(new Vector2(x, y));

            points.Add(new Vector2(x, -y));

            points.Add(new Vector2(-x, -y));

            points.Add(new Vector2(-x, y));
            //设置坐标点
            foreach (var point in points)
            {
                helper.AddVert(new UIVertex()
                {
                    position = point,
                    color = color,
                    uv0 = new Vector2(point.x / space.x + 0.5f, point.y / space.y + 0.5f),
                });
            }
            int count = points.Count;
            //自定义三角形
            for (int i = 1; i < count - 1; i++)
            {
                helper.AddTriangle(i, 0, i + 1);
            }
        }

        public override Texture mainTexture => sprite == null ? s_WhiteTexture : sprite.texture;
    }
}