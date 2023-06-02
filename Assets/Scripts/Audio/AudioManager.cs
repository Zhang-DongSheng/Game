using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Renewable;
using UnityEngine.SceneManagement;

namespace Game.Audio
{
    public sealed class AudioManager : MonoSingleton<AudioManager>
    {
        private readonly Dictionary<AudioEnum, AudioSourceInformation> audios = new Dictionary<AudioEnum, AudioSourceInformation>();

        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

        private AudioListener listener;

        private void Awake()
        {
            AudioSource[] sources = transform.GetComponentsInChildren<AudioSource>();

            AudioSource source;

            int index = 0;

            foreach (var ae in Enum.GetValues(typeof(AudioEnum)))
            {
                if (sources.Length > index++)
                {
                    source = sources[index - 1];
                }
                else
                {
                    GameObject go = new GameObject(ae.ToString());
                    go.transform.SetParent(transform);
                    source = go.AddComponent<AudioSource>();
                }
                audios.Add((AudioEnum)ae, new AudioSourceInformation((AudioEnum)ae, source));
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void PlayMusic(string sound, bool loop = false)
        {
            Play(AudioEnum.Music, sound, loop);
        }

        public void PlayEffect(string sound)
        {
            Play(AudioEnum.Sound, sound, false);
        }

        public void Play(AudioEnum key, string sound, bool loop = false)
        {
            if (string.IsNullOrEmpty(sound)) return;

            if (clips.ContainsKey(sound))
            {
                Play(key, clips[sound], loop);
            }
            else if (AudioConfig.list.ContainsKey(sound))
            {
                AudioClip clip = Resources.Load<AudioClip>(AudioConfig.list[sound]);

                Store(sound, clip); Play(key, clip, loop);
            }
            else
            {
                RenewableResource.Instance.Get(new RenewableRequest(sound), (handle) =>
                {
                    AudioClip clip = handle.Get<AudioClip>();

                    Store(sound, clip); Play(key, clips[sound], loop);
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
            else if (AudioConfig.list.ContainsKey(sound))
            {
                AudioClip clip = Resources.Load<AudioClip>(AudioConfig.list[sound]);
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
                audios[key].source.Pause();
            }
        }

        public void Stop(AudioEnum key)
        {
            if (audios.ContainsKey(key) && audios[key].source.isPlaying)
            {
                audios[key].source.Stop();
                audios[key].source.clip = null;
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
                audios[key].SetVolume(volume);
            }
        }

        public void SetMute(AudioEnum key, bool mute)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].SetMute(mute);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            listener = GameObject.FindObjectOfType<AudioListener>();

            if (listener == null)
            {
                Debuger.LogError(Author.Sound, "音频监听器未就绪！");
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
                audios[key].source.loop = loop;

                audios[key].source.clip = clip;

                audios[key].source.Play();
            }
        }

        private void PlayDelayed(AudioEnum key, AudioClip clip, float delay)
        {
            if (audios.ContainsKey(key))
            {
                audios[key].source.loop = false;

                audios[key].source.clip = clip;

                audios[key].source.PlayDelayed(delay);
            }
        }
    }
}