namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    public class ImageBlur : MaskableGraphic
    {
        [SerializeField, Range(0, 1)] private float blur;

        [SerializeField] private Sprite sprite;

        protected override void Awake()
        {
            if (material == null)
            {
                material = new Material(Shader.Find("Blur/Gaussian"));
            }
            material.SetFloat("_Blur", blur);
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (material != null)
            {
                material.SetFloat("_Blur", blur);
            }
        }
#endif
        public override Texture mainTexture => sprite == null ? s_WhiteTexture : sprite.texture;
    }
}