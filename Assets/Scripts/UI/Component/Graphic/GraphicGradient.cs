using Game.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// Í¼ÐÎ½¥±ä
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class GraphicGradient : BaseMeshEffect
    {
        public Direction direction;

        public GradientStyle style;

        public Color from = Color.white;

        public Color to = Color.white;

        public float rotation;

        private UIVertex vertex = new UIVertex();

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!isActiveAndEnabled)
                return;

            var rect = default(Rect);

            switch (style)
            {
                case GradientStyle.Rect:
                    rect = graphic.rectTransform.rect;
                    break;
                case GradientStyle.Split:
                    rect.Set(0, 0, 1, 1);
                    break;
                case GradientStyle.Fit:
                    {
                        // Fit to contents.
                        rect.xMin = rect.yMin = float.MaxValue;
                        rect.xMax = rect.yMax = float.MinValue;
                        for (var i = 0; i < vh.currentVertCount; i++)
                        {
                            vh.PopulateUIVertex(ref vertex, i);
                            rect.xMin = Mathf.Min(rect.xMin, vertex.position.x);
                            rect.yMin = Mathf.Min(rect.yMin, vertex.position.y);
                            rect.xMax = Mathf.Max(rect.xMax, vertex.position.x);
                            rect.yMax = Mathf.Max(rect.yMax, vertex.position.y);
                        }
                        break;
                    }
            }

            // Gradient rotation.
            var rad = rotation * Mathf.Deg2Rad;

            var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            var localMatrix = new Matrix2x3(rect, dir.x, dir.y);

            for (var i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);

                Vector2 normalizedPos = localMatrix * vertex.position;

                Color color = Color.LerpUnclamped(from, to, normalizedPos.y);

                vertex.color *= color;

                vh.SetUIVertex(vertex, i);
            }
        }

        public enum GradientStyle
        {
            Rect,
            Fit,
            Split,
        }
    }
}