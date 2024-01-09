using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 图画
        /// </summary>
        public static class Drawing
        {
            public static Texture2D Texture2D(System.Drawing.Bitmap bitmap)
            {
                int width = bitmap.Width;

                int height = bitmap.Height;

                Texture2D texture = new Texture2D(width, height);

                Color color = new Color(0, 0, 0, 1);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var pixel = bitmap.GetPixel(x, y);
                        color.r = pixel.R;
                        color.g = pixel.G;
                        color.b = pixel.B;
                        color.a = pixel.A;
                        texture.SetPixel(x, y, color);
                    }
                }
                texture.Apply();

                return texture;
            }
            /// <summary>
            /// 生成Texture2D
            /// </summary>
            public static Texture2D Texture2D(Texture texture)
            {
                int width = texture.width, height = texture.height;

                Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);

                RenderTexture active = RenderTexture.active;

                RenderTexture render = RenderTexture.GetTemporary(width, height, 32);

                Graphics.Blit(texture, render);

                RenderTexture.active = render;

                texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);

                texture2D.Apply();

                RenderTexture.active = active;

                RenderTexture.ReleaseTemporary(render);

                return texture2D;
            }
            /// <summary>
            /// 生成Sprite
            /// </summary>
            public static Sprite Sprite(Texture2D texture)
            {
                return UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
    }
}