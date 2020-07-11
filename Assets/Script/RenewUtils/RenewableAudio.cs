using System;
using UnityEngine;

namespace UI
{
    public class RenewableAudio : RenewableBase
    {
        [SerializeField] private bool loop;

        private AudioClip clip;

        protected override DownloadFileType fileType { get { return DownloadFileType.Audio; } }

        public void PlayMusic(string key, string url = "", string extra = "", Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (key != this.key)
            {
                if (RenewablePool.Instance.Exist(cache, key))
                {
                    clip = RenewablePool.Instance.Pop<AudioClip>(cache, key);

                    Play(clip);
                }
                else
                {
                    Get(key, url, extra, callBack);
                }
            }
            else
            {
                Play(clip);
            }
        }

        protected override void Create(byte[] buffer, UnityEngine.Object content)
        {
            if (content != null)
            {
                RenewablePool.Instance.Push(cache, key, content);

                clip = content as AudioClip;

                Play(clip);
            }
            else
            {
                //encoding audio clip ...

                //play ...
            }
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;

            //play ...
        }
    }
}