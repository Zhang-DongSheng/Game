using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        private readonly Dictionary<SourceEnum, AudioSource> sources = new Dictionary<SourceEnum, AudioSource>();

        private readonly Dictionary<ListenerEnum, AudioListener> listeners = new Dictionary<ListenerEnum, AudioListener>();

        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>(); 

        public void Init()
        {

        }

        public void SwitchListener(ListenerEnum key)
        {
            foreach (var listener in listeners)
            {
                if (listener.Value != null)
                {
                    listener.Value.enabled = listener.Key == key;
                }
            }
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
            if (sources.ContainsKey(key))
            {
                sources[key].Play();
            }
        }

        private AudioClip LoadAudioClip(string sound)
        {
            AudioClip clip = null;


            return clip;
        }
    }
}
