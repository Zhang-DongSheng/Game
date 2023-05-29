using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public static class AudioConfig
    {
        public readonly static Dictionary<string, string> list = new Dictionary<string, string>()
        {
            { "test", "Audio/test" },
        };
    }

    public class AudioSourceInformation
    {
        public string key;

        public bool mute;

        public float volume;

        public AudioSource source;

        public AudioSourceInformation(AudioEnum ae, AudioSource source)
        {
            this.source = source;

            key = ae.ToString();

            mute = PlayerPrefs.GetInt(string.Format("{0}_MUTE", key)) == 1;

            volume = PlayerPrefs.GetFloat(string.Format("{0}_VOLUME", key));

            source.volume = volume;

            source.mute = mute;
        }

        public void SetMute(bool mute)
        {
            source.mute = mute;

            PlayerPrefs.SetInt(string.Format("{0}_MUTE", key), mute ? 1 : 0);
        }

        public void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);

            source.volume = volume;

            PlayerPrefs.SetFloat(string.Format("{0}_VOLUME", key), volume);
        }
    }

    public enum AudioEnum
    {
        Music,
        Effect,
        Special,
    }
}