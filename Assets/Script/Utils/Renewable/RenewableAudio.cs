using System;
using UnityEngine.Renewable;

namespace UnityEngine
{
    public class RenewableAudio : RenewableBase
    {
        [SerializeField] private bool loop;

        private AudioClip m_clip;

        protected override DownloadFileType fileType { get { return DownloadFileType.Audio; } }

        public void PlayMusic(string key, int order = 0, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.current = key;

            this.key = string.Empty;

            if (RenewablePool.Instance.Exist(cache, key, string.Empty))
            {
                m_clip = RenewablePool.Instance.Pop<AudioClip>(cache, key);

                this.key = key; callBack?.Invoke(); Play(m_clip);
            }
            else
            {
                Get(key, null, order, callBack);
            }
        }

        protected override void Create(RenewableDownloadHandler handle)
        {
            AudioClip clip = null;

            if (RenewablePool.Instance.Exist(cache, handle.key, handle.secret))
            {
                clip = RenewablePool.Instance.Pop<AudioClip>(cache, handle.key);
            }
            else
            {
                if (handle.source != null)
                {
                    clip = handle.Get<AudioClip>();

                    RenewablePool.Instance.Push(cache, handle.key, handle.secret, handle.recent, clip);
                }
                else
                {
                    Debug.LogWarningFormat("{0} 无法解析！", handle.key);
                }
            }

            if (this == null) return;

            if (current != handle.key) return;

            Play(clip);
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;

            if (gameObject.activeSelf == false) return;

            m_clip = clip;

            //play...
        }
    }
}