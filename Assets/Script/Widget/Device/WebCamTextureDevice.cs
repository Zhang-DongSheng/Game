using System.Collections;
using UnityEngine;

namespace Device
{
    /// <summary>
    /// 摄像头
    /// </summary>
    public sealed class WebCamTextureDevice : Device<WebCamTextureDevice>
    {
        private WebCamDevice[] devices;

        protected override void Initialized()
        {
            
        }

        public override void Begin()
        {
            if (apply)
            {
                ApplyPermissions(); return;
            }

            if (devices.Length == 0) return;

            string device = devices[0].name;

            if (Camera == null)
            {
                Camera = new WebCamTexture(device, Screen.width, Screen.height, 30)
                {
                    wrapMode = TextureWrapMode.Repeat,
                };
            }
            Camera.Play();
        }

        public override void Stop()
        {
            if (Camera != null && Camera.isPlaying)
            {
                Camera.Stop();
            }
            Camera = null;
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator Permissions()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

            apply = false;

            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                int index = 0;

                while (WebCamTexture.devices.Length == 0 && index < 300)
                {
                    yield return new WaitForEndOfFrame(); index++;
                }
                devices = WebCamTexture.devices;

                if (devices.Length == 0)
                {
                    Debuger.LogWarning(Author.Device, "未找到摄像头！");
                }
                else
                {
                    Begin();
                }
            }
            else
            {
                Debuger.LogWarning(Author.Device, "未获得读取摄像头权限！");
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (Camera != null)
            {
                if (pause)
                {
                    Camera.Pause();
                }
                else
                {
                    Camera.Play();
                }
            }
        }

        public WebCamTexture Camera { get; private set; }
    }
}