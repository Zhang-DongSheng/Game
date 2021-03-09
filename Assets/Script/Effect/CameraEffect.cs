using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Effect
{
    [RequireComponent(typeof(Camera))]
    public class CameraEffect : MonoBehaviour
    {
        [SerializeField] private Material material;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material != null)
            {

                Graphics.Blit(source, destination, material);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }
    }
}