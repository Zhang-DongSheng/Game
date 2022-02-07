using System;
using System.Collections.Generic;
using UnityEngine.Renewable;

namespace UnityEngine.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
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

                clips.Add(sound, clip);

                Play(key, clips[sound], loop);
            }
            else
            {
                RenewableResource.Instance.Get(new RenewableRequest(sound), (handle) =>
                {
                    AudioClip clip = handle.Get<AudioClip>();

                    clips.Add(sound, clip);

                    Play(key, clips[sound], loop);
                }, () =>
                {
                    Debuger.LogWarning(Author.Sound, $"{ sound }该音频未录入下载列表!");
                });
            }
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
    }
}