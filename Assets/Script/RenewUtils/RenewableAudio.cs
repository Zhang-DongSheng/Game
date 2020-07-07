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
            if (this.key == key)
            {
                Play(clip);
            }
            Get(key, url, extra, callBack);
        }

        protected override void Create(byte[] buffer, UnityEngine.Object content)
        {
            if (content != null)
            {
                clip = content as AudioClip;

                Play(clip);
            }
            else
            {
                //byte[] to AudioClip

                Play(clip);
            }
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;

            //play music
        }
    }
}