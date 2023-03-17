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

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}