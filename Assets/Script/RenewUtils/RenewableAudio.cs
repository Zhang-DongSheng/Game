using System;
using UnityEngine;

namespace UI
{
    public class RenewableAudio : RenewableBase
    {
        [SerializeField] private bool loop;

        private AudioClip clip;

        protected override DownloadFileType fileType { get { return DownloadFileType.Audio; } }

        public void PlayMusic(string key, string url = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (this.key == key && clip != null)
            {
                callBack?.Invoke(); Play(clip);
                return;
            }
            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key))
            {
                clip = RenewablePool.Instance.Pop<AudioClip>(cache, key);

                this.key = key; callBack?.Invoke(); Play(clip);
            }
            else
            {
                Get(key, url, callBack);
            }
        }

        protected override void Create(string key, byte[] buffer, UnityEngine.Object content)
        {
            if (content != null)
            {
                RenewablePool.Instance.Push(cache, key, content);

                clip = content as AudioClip;

                if (current != key) return;

                Play(clip);
            }
            else
            {
                Debug.LogWarningFormat("{0} 无法解析！", key);
            }
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;

            if (!Active) return;

            //Play Audio clip!
        }
    }
}