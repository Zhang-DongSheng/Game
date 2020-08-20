using System;
using UnityEngine;

namespace UI
{
    public class RenewableAudio : RenewableBase
    {
        [HideInInspector] public bool enable;

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

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_clip = RenewablePool.Instance.Pop<AudioClip>(cache, key);

                this.key = key; callBack?.Invoke(); Play(m_clip);
            }
            else
            {
                Get(key, url, callBack);
            }
        }

        protected override void Create(string key, byte[] buffer, UnityEngine.Object content, string secret)
        {
            AudioClip clip = null;

            if (RenewablePool.Instance.Exist(cache, key, secret))
            {
                clip = RenewablePool.Instance.Pop<AudioClip>(cache, key);
            }
            else
            {
                if (content != null)
                {
                    clip = content as AudioClip;

                    RenewablePool.Instance.Push(cache, key, secret, clip);
                }
                else
                {
                    Debug.LogWarningFormat("{0} 无法解析！", key);
                }
            }

            if (current != key) return;

            Play(clip);
        }

        private void Play(AudioClip clip)
        {
            if (!enable || clip == null) return;

            m_clip = clip;

            //Play Audio clip!
        }
    }
}