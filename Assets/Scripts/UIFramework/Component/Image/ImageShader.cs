namespace UnityEngine.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(Image))]
    public class ImageShader : MonoBehaviour
    {
        [SerializeField] private Shader shader;

        private MaskableGraphic graphic;

        private void Start()
        {
            SetMaterial();
        }

        private void OnValidate()
        {
            SetMaterial();
        }

        public void SetMaterial()
        {
            graphic = GetComponent<MaskableGraphic>();

            if (graphic != null)
            {
                if (graphic.material == null || graphic.material.shader != shader)
                {
                    if (shader != null)
                        graphic.material = new Material(shader);
                }
            }
            else
            {
                Debug.LogError("Please attach component to a Graphical UI component");
            }
        }
    }
}