using Game.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// 图形反转
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class GraphicReversal : BaseMeshEffect
    {
        [SerializeField] private Axis axis;

        public override void ModifyMesh(VertexHelper verts)
        {
            RectTransform rt = this.transform as RectTransform;

            for (int i = 0; i < verts.currentVertCount; ++i)
            {
                UIVertex vertex = new UIVertex();

                verts.PopulateUIVertex(ref vertex, i);

                vertex.position = new Vector3(vertex.position.x, vertex.position.y, vertex.position.z);

                switch (axis)
                {
                    case Axis.Horizontal:
                        vertex.position.x += (rt.rect.center.x - vertex.position.x) * 2;
                        break;
                    case Axis.Vertical:
                        vertex.position.y += (rt.rect.center.y - vertex.position.y) * 2;
                        break;
                    case Axis.Custom:
                        vertex.position.x += (rt.rect.center.x - vertex.position.x) * 2;
                        vertex.position.y += (rt.rect.center.y - vertex.position.y) * 2;
                        break;
                }
                verts.SetUIVertex(vertex, i);
            }
        }
    }
}