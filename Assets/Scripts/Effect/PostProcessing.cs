 using System.Collections.Generic;
using UnityEngine;

namespace Game.Effect
{
    [RequireComponent(typeof(Camera))]
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField] private Shader _shader;

        [SerializeField] protected List<PostProperty> properties;

        protected Material material;
        protected Shader shader
        {
            get
            {
                return _shader;
            }
            set
            {
                if (_shader != value)
                {
                    _shader = value;
                }
                Material();
            }
        }

        protected virtual void Awake()
        {
            Material();
        }

        protected virtual void OnValidate()
        {
            shader = _shader;
        }

        protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
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
                if (material != null)
                {
                    if (material.shader != shader)
                    {
                        Destroy(material); material = null;
                        Material();
                    }
                }
                else
                {
                    material = new Material(shader)
                    {
                        hideFlags = HideFlags.HideAndDontSave,
                    };
                }
            }
        }

        protected virtual void Compute(Material material)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                properties[i].Procession(ref material);
            }
        }

        public void Reload(string name)
        {
            this.shader = Shader.Find(name);
        }

        public void Reload(Shader shader)
        {
            this.shader = shader;
        }
    }
}