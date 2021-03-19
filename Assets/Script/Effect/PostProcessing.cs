using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Effect
{
    [RequireComponent(typeof(Camera))]
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField] private Material material;

        private void Awake()
        {
            
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

        protected virtual void Compute(Material material)
        { 
            
        }
    }
}