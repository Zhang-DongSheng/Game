using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 图像
        /// </summary>
        public static class Image
        {
            public static byte[] Serialize(System.Drawing.Image image)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, image.RawFormat);

                    byte[] buffer = new byte[stream.Length];

                    stream.Seek(0, SeekOrigin.Begin);

                    stream.Read(buffer, 0, buffer.Length);

                    return buffer;
                }
            }

            public static byte[] Serialize(Bitmap bitmap)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, bitmap.RawFormat);

                    byte[] buffer = new byte[stream.Length];

                    stream.Seek(0, SeekOrigin.Begin);

                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));

                    return buffer;
                }
            }

            public static System.Drawing.Image Deserialize(byte[] buffer)
            {
                MemoryStream stream = new MemoryStream(buffer);

                var image = System.Drawing.Image.FromStream(stream);

                return image;
            }

            public static List<Sprite> ImageToSprites(System.Drawing.Image image)
            {
                var sprites = new List<Sprite>();

                if (image == null) return sprites;

                FrameDimension frame = new FrameDimension(image.FrameDimensionsList[0]);

                int framCount = image.GetFrameCount(frame);//获取维度帧数

                for (int i = 0; i < framCount; ++i)
                {
                    image.SelectActiveFrame(frame, i);

                    Bitmap framBitmap = new Bitmap(image.Width, image.Height);

                    using (System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(framBitmap))
                    {
                        graphic.DrawImage(image, Point.Empty);
                    }
                    var texture = new Texture2D(framBitmap.Width, framBitmap.Height, TextureFormat.ARGB32, true);

                    texture.LoadImage(Serialize(framBitmap));

                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    sprites.Add(sprite);
                }
                return sprites;
            }
        }
    }
}
