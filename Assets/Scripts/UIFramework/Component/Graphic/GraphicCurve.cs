using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 图形弯曲
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class GraphicCurve : BaseMeshEffect
    {
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private float ratio = 1.0f;

        private readonly List<UIVertex> vertexs = new List<UIVertex>();

        private RectTransform rect;

        private UIVertex vertex;

        public override void ModifyMesh(VertexHelper helper)
        {
            if (!IsActive()) return;

            helper.GetUIVertexStream(vertexs);

            for (int i = 0; i < vertexs.Count; i++)
            {
                vertex = vertexs[i];

                vertex.position.y += curve.Evaluate(Progress(vertex.position.x)) * ratio - ratio * 0.5f;

                vertexs[i] = vertex;
            }
            helper.AddUIVertexTriangleStream(vertexs);
        }

        private float Progress(float position)
        {
            if (rect != null || TryGetComponent(out rect))
            {
                if (rect.rect.width == 0) return 0;

                position += rect.rect.width * rect.pivot.x;

                return position / rect.rect.width;
            }
            return 0;
        }
    }
}