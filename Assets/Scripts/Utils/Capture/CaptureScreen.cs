#if UNITY_ANDROID
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// ¼��
    /// </summary>
    public class CaptureScreen : MonoBehaviour
    {
        [SerializeField] private RawImage image;

        [SerializeField] private RectInt rect;

        private byte[] buffer;

        private bool over;

        private Thread thread;

        private Texture2D texture;

        private void Start()
        {
            thread = new Thread(new ThreadStart(Capture))
            {
                IsBackground = true,
            };
            thread.Start();
        }

        private void Update()
        {
            if (buffer != null && over)
            {
                Create(buffer);
            }
        }

        private void Capture()
        {
            try
            {
                while (true)
                {
                    if (!over)
                    {
                        using (MemoryStream memory = new MemoryStream())
                        {
                            Compress(Screen(), memory);

                            buffer = memory.ToArray();

                            over = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.Script, e);
            }
        }

        private Bitmap Screen()
        {
            Bitmap bitmap = new Bitmap(rect.width, rect.height);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            graphics.CopyFromScreen(rect.x, rect.y, 0, 0, bitmap.Size);

            return bitmap;
        }

        private void Compress(Bitmap bitmap, MemoryStream memory)
        {
            try
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.Script, e);
            }
        }

        private void Create(byte[] buffer)
        {
            if (texture == null)
            {
                texture = new Texture2D(rect.width, rect.height);
            }
            texture.LoadImage(buffer);

            image.texture = texture;

            image.SetNativeSize();

            over = false;
        }

        private void OnDestroy()
        {
            if (thread != null)
            {
                thread.Abort(); thread = null;
            }
            if (texture != null)
            {
                GameObject.Destroy(texture); texture = null;
            }
        }
    }
}
#endif