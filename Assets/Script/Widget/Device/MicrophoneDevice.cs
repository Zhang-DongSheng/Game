using System.Collections;
using UnityEngine;

namespace Device
{
    /// <summary>
    /// 麦克风
    /// </summary>
    public class MicrophoneDevice : Device<MicrophoneDevice>
    {
        private string[] devices = null;

        private string device = null;

        public float sensitivity = 100;

        public float loudness = 0;

        const int HEADER_SIZE = 44;

        const int RECORD_TIME = 10;

        const int RECORD_RATE = 44100; //录音采样率

        protected override void Initialized()
        {
            devices = Microphone.devices;

            if (devices.Length == 0)
            {
                Debuger.LogWarning(Author.Device, "未找到麦克风！");
            }
            else
            {
                Debuger.Log(Author.Device, "麦克风准备就绪！");
            }
        }

        public override void Begin()
        {
            if (apply)
            {
                ApplyPermissions(); return;
            }

            if (devices.Length == 0) return;

            device = devices[0];

            Microphone.End(device);//录音时先停掉录音，录音参数为null时采用默认的录音驱动

            Clip = Microphone.Start(device, false, RECORD_TIME, RECORD_RATE);
        }

        public override void Stop()
        {
            if (Microphone.IsRecording(device))
            {
                Microphone.End(device);
            }
        }

        public override void Save()
        {
            float[] buffer = new float[Clip.samples];

            Clip.SetData(buffer, 1);
        }

        protected override IEnumerator Permissions()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

            apply = false;

            if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                int index = 0;

                while (WebCamTexture.devices.Length == 0 && index < 300)
                {
                    yield return new WaitForEndOfFrame(); index++;
                }
                devices = Microphone.devices;

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

        public AudioClip Clip { get; private set; }
    }
}