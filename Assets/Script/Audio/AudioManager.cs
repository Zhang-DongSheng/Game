using System;
using System.Collections.Generic;
using UnityEngine.Renewable;

namespace UnityEngine.Audio
{
    public sealed class AudioManager : MonoSingleton<AudioManager>
    {
        private readonly Dictionary<string, AudioInformation> information = new Dictionary<string, AudioInformation>();

        private readonly Dictionary<AudioEnum, AudioSource> audios = new Dictionary<AudioEnum, AudioSource>();

        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

        private AudioListener listener;

        private void Awake()
        {
            AudioSource[] sources = transform.GetComponentsInChildren<AudioSource>();

            AudioSource source;

            int index = 0;

            foreach (var se in Enum.GetValues(typeof(AudioEnum)))
            {
                if (sources.Length > index++)
                {
                    source = sources[index - 1];
                }
                else
                {
                    GameObject go = new GameObject(se.ToString());
                    go.transform.SetParent(transform);
                    source = go.AddComponent<AudioSource>();
                }
                audios.Add((AudioEnum)se, source);
            }

            listener = GameObject.FindObjectOfType<AudioListener>();

            if (listener == null)
            {
                listener = gameObject.AddComponent<AudioListener>();
            }
        }

        public void PlayMusic(string sound, bool loop = false)
        {
            Play(AudioEnum.Music, sound, loop);
        }

        public void PlayEffect(string sound)
        {
            Play(AudioEnum.Effect, sound, false);
        }

        public void Play(AudioEnum key, string sound, bool loop = false)
        {
            if (clips.ContainsKey(sound))
            {
                Play(key, clips[sound], loop);
            }
            else if (information.ContainsKey(sound))
            {
                AudioClip clip = Resources.Load<AudioClip>(information[sound].path);
                Store(sound, clip); Play(key, clip, loop);
            }
            else
            {
                RenewableResource.Instance.Get(new RenewableRequest(sound), (handle) =>
                {
                    AudioClip clip = handle.Get<AudioClip>();
                    Store(sound, clip); Play(key, clip, loop);
                }, () =>
                {
                    Debuger.LogWarning(Author.Sound, $"{ sound }该音频未录入下载列表!");
                });
            }
        }

        public void PlayDelayed(AudioEnum key, string sound, float delay = 0)
        {
            if (clips.ContainsKey(sound))
            {
                PlayDelayed(key, clips[sound], delay);
            }
            else if (information.ContainsKey(sound))
            {
                AudioClip clip = Resources.Load<AudioClip>(information[sound].path);
                Store(sound, clip); PlayDelayed(key, clip, delay);
            }
            else
            {
                RenewableResource.Instance.Get(new RenewableRequest(sound), (handle) =>
                {
                    AudioClip clip = handle.Get<AudioClip>();
                    Store(sound, clip); PlayDelayed(key, clip, delay);
                }, () =>
                {
                    Debuger.LogWarning(Author.Sound, $"{ sound }该音频未录入下载列表!");
                });
            }
        }

        public void Pause(AudioEnum key)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].Pause();
            }
        }

        public void Stop(AudioEnum key)
        {
            if (audios.ContainsKey(key) && audios[key].isPlaying)
            {
                audios[key].Stop();
                audios[key].clip = null;
            }
        }

        public void StopAll()
        {
            foreach (var se in Enum.GetValues(typeof(AudioEnum)))
            {
                Stop((AudioEnum)se);
            }
        }

        public void SetVolume(AudioEnum key, float volume)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].volume = Mathf.Clamp01(volume);
            }
        }

        public void SetMute(AudioEnum key, bool mute)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].mute = mute;
            }
        }

        private void Store(string key, AudioClip clip)
        {
            clips.Add(key, clip);
        }

        private void Play(AudioEnum key, AudioClip clip, bool loop)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].loop = loop;

                audios[key].clip = clip;

                audios[key].Play();
            }
        }

        private void PlayDelayed(AudioEnum key, AudioClip clip, float delay)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].loop = false;

                audios[key].clip = clip;

                audios[key].PlayDelayed(delay);
            }
        }
    }
}