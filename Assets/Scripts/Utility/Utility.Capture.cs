using System;
using System.IO;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 屏幕捕获
        /// </summary>
        public static class Capture
        {
            public static Texture2D Screenshot()
            {
                return ScreenCapture.CaptureScreenshotAsTexture();
            }

            public static Texture2D Screenshot(Rect rect, TextureFormat format = TextureFormat.RGB24)
            {
                Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, format, false);

                texture.ReadPixels(rect, 0, 0);

                texture.Apply();

                return texture;
            }

            public static Texture2D Screenshot(Camera camera, Rect rect, TextureFormat format = TextureFormat.RGB24, bool onetime = false)
            {
                if (camera == null) return null;

                if (camera.targetTexture == null)
                {
                    camera.targetTexture = new RenderTexture((int)rect.width, (int)rect.height, 0);
                }
                camera.Render();

                RenderTexture.active = camera.targetTexture;

                Texture2D texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, format, false);

                texture.ReadPixels(rect, 0, 0);

                texture.Apply();

                if (onetime)
                {
                    if (camera.targetTexture != null)
                    {
                        UnityEngine.GameObject.Destroy(camera.targetTexture);
                    }
                    camera.targetTexture = null;

                    RenderTexture.active = null;
                }
                return texture;
            }

            public static void CreatePng(Texture2D texture, string path)
            {
                if (texture == null) return;

                try
                {
                    byte[] buffer = texture.EncodeToPNG();

                    string direction = Path.GetDirectoryName(path);

                    Document.CreateDirectory(direction);

                    File.WriteAllBytes(path, buffer);
                }
                catch (Exception e)
                {
                    Debuger.LogException(Author.File, e);
                }
            }
        }
    }
}