using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static class Destroy
    {
        public static void Release(Object target)
        {
            if (target is GameObject go)
            {
                SpriteRenderer[] sprites = go.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (sprites[i].sprite != null)
                    {
                        if (sprites[i].sprite.texture != null)
                        {
                            Resources.UnloadAsset(sprites[i].sprite.texture);
                        }
                        Object.DestroyImmediate(sprites[i].sprite, true);
                    }
                }

                Image[] images = go.GetComponentsInChildren<Image>();

                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i].sprite != null)
                    {
                        if (sprites[i].sprite.texture != null)
                        {
                            Resources.UnloadAsset(images[i].sprite.texture);
                        }
                        Object.DestroyImmediate(images[i].sprite, true);
                    }
                }

                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i].sharedMaterial != null)
                    {
                        if (renderers[i].sharedMaterial.mainTexture != null)
                        {
                            Resources.UnloadAsset(renderers[i].sharedMaterial.mainTexture);
                        }
                        Object.DestroyImmediate(renderers[i].sharedMaterial, true);
                    }
                }

                ParticleSystem[] particles = go.GetComponentsInChildren<ParticleSystem>();

                for (int i = 0; i < particles.Length; i++)
                {
                    if (particles[i].TryGetComponent(out Renderer renderer))
                    {
                        if (renderer.sharedMaterial != null)
                        {
                            if (renderer.sharedMaterial.mainTexture != null)
                            {
                                Resources.UnloadAsset(renderer.sharedMaterial.mainTexture);
                            }
                            Object.DestroyImmediate(renderer.sharedMaterial, true);
                        }
                    }
                }
            }
        }
    }
}