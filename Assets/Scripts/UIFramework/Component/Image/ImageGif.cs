using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class ImageGif : MonoBehaviour
    {
        [SerializeField] private Image component;

        [SerializeField] private Texture2D source;

        [SerializeField] private float speed = 5f;

        private readonly List<Sprite> sprites = new List<Sprite>();

        private int index, count;

        private float timer;

        private void Awake()
        {
            if (component == null)
                component = GetComponent<Image>();

            System.Drawing.Image image = System.Drawing.Image.FromFile(Application.dataPath + "/Package/Atlas/Gif/giphy.gif");

            ReloadGif(image);
        }

        private void Update()
        {
            if (component == null) return;

            if (timer > count)
                timer = 0;
            else
                timer += Time.deltaTime * speed;

            if (count > 1)
            {
                index = (int)timer % count;

                component.sprite = sprites[index];
            }
        }

        private void ReloadGif(System.Drawing.Image image)
        {
            if (image == null) return;

            sprites.Clear();

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
                Texture2D frameTexture2D = new Texture2D(framBitmap.Width, framBitmap.Height, TextureFormat.ARGB32, true);
                
                frameTexture2D.LoadImage(Bitmap2Byte(framBitmap));

                var sprite = Sprite.Create(frameTexture2D, new Rect(0, 0, frameTexture2D.width, frameTexture2D.height), new Vector2(0.5f, 0.5f));

                sprites.Add(sprite);
            }
            count = sprites.Count;
        }

        private byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // 将bitmap 以png格式保存到流中
                bitmap.Save(stream, ImageFormat.Png);
                // 创建一个字节数组，长度为流的长度
                byte[] data = new byte[stream.Length];
                // 重置指针
                stream.Seek(0, SeekOrigin.Begin);
                // 从流读取字节块存入data中
                stream.Read(data, 0, Convert.ToInt32(stream.Length));

                return data;
            }
        }

        public void SetGif(Texture2D texture)
        {
            this.source = texture;
            // 想法转过去
            //ReloadGif(image);
        }
    }
}