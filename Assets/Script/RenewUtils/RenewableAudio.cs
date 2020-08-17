using System;
using UnityEngine;

namespace UI
{
    public class RenewableAudio : RenewableBase
    {
        [SerializeField] private bool loop;

        private AudioClip m_clip;

        protected override DownloadFileType fileType { get { return DownloadFileType.Audio; } }

        public void PlayMusic(string key, string url = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            if (this.key == key && m_clip != null)
            {
                callBack?.Invoke(); Play(m_clip);
                return;
            }
            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key))
            {
                m_clip = RenewablePool.Instance.Pop<AudioClip>(cache, key);

                this.key = key; callBack?.Invoke(); Play(m_clip);
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
                AudioClip clip = content as AudioClip;

                RenewablePool.Instance.Push(cache, key, content);

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
            if (!Active || clip == null) return;

            m_clip = clip;

            //Play Audio clip!
        }
    }
}