using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 属性图
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class ItemAttributes : MaskableGraphic
    {
        [SerializeField] private Sprite sprite;

        [SerializeField] private float radius = 100;

        private Vector2 space = new Vector2();

        private readonly List<Vector2> points = new List<Vector2>();

        public void Refresh(List<float> parameters)
        {
            points.Clear();

            int count = parameters.Count;

            float ratio, angle = 360f / count;

            for (int i = 0; i < count; i++)
            {
                ratio = Mathf.Clamp01(parameters[i]) * radius;

                points.Add(new Vector2()
                {
                    x = Mathf.Sin(i * angle * Mathf.Deg2Rad) * ratio,
                    y = Mathf.Cos(i * angle * Mathf.Deg2Rad) * ratio,
                });
            }
            SetAllDirty();
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
