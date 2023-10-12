namespace UnityEngine.UI
{
    /// <summary>
    /// 空渲染
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class GraphicEmpty : Graphic
    {
        protected GraphicEmpty()
        {
            useLegacyMeshGeneration = false;
        }

        public override void SetMaterialDirty() { return; }

        public override void SetVerticesDirty() { return; }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}