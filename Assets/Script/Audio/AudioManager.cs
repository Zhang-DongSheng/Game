using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        private readonly Dictionary<SourceEnum, AudioSource> sources = new Dictionary<SourceEnum, AudioSource>();

        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

        public void Init()
        {

        }

        public void PlayMusic(string sound, bool loop)
        {
            Play(SourceEnum.Music, sound, loop);
        }

        public void PlayEffect(string sound)
        {
            Play(SourceEnum.Effect, sound, false);
        }

        private void Play(SourceEnum key, string sound, bool loop)
        {
            if (clips.ContainsKey(sound))
            {
                Play(key, clips[sound], loop);
            }
            else
            {

            }
        }

        private void Play(SourceEnum key, AudioClip clip, bool loop)
        {
            if (sources.ContainsKey(key))
            {
                sources[key].loop = loop;

                sources[key].clip = clip;

                sources[key].Play();
            }
        }

        private AudioClip LoadAudioClip(string sound)
        {
            AudioClip clip = null;

            //load clip

            return clip;
        }
    }
}