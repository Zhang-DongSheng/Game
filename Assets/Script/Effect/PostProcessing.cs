using UnityEngine;

namespace Game.Effect
{
    [RequireComponent(typeof(Camera))]
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField] private Shader shader;

        private Material material;

        private void Awake()
        {
            Material();
        }

        private void OnValidate()
        {
            Material();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material != null)
            {
                Compute(material);

                Graphics.Blit(source, destination, material);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }

        protected virtual void Material()
        {
            if (shader != null && shader.isSupported)
            {
                if (material != null && material.shader == shader) { }
                else
                {
                    material = new Material(shader);
                }
            }
        }

        protected virtual void Compute(Material material)
        {
            
        }
    }
}