using Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEditor.Utils
{
    public static class SpriteUtils
    {
        public static Texture2D[] GetTexturesFromSpriteAtlas(SpriteAtlas atlas)
        {
            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static;

            var func = typeof(U2D.SpriteAtlasExtensions).GetMethod("GetPreviewTextures", flags);

            var textures = func.Invoke(null, new object[] { atlas }) as Texture2D[];

            return textures;
        }

        public static void Gray(Texture2D texture, string path)
        {
            int width = texture.width;

            int height = texture.height;

            var clone = new Texture2D(width, height, TextureFormat.ARGB32, true);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var color = texture.GetPixel(i, j);

                    var gray = color.Gray();

                    clone.SetPixel(i, j, gray);
                }
            }
            Utility.Capture.CreatePng(clone, path);

            AssetDatabase.Refresh();
        }

        public static void Compress(Texture2D texture, string path)
        {
            
        }
    }
}
