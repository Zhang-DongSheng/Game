using Game.Data;
using UnityEngine;

namespace Game.Audio
{
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

            mute = GlobalVariables.Get<bool>(string.Format("{0}_{1}", Const.AUDIO_MUTE, key));

            volume = GlobalVariables.Get<float>(string.Format("{0}_{1}", Const.AUDIO_VOLUME, key));

            source.volume = volume;

            source.mute = mute;
        }

        public void SetMute(bool mute)
        {
            source.mute = mute;

            GlobalVariables.Set(string.Format("{0}_{1}", Const.AUDIO_MUTE, key), mute);
        }

        public void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);

            source.volume = volume;

            GlobalVariables.Set(string.Format("{0}_{1}", Const.AUDIO_VOLUME, key), volume);
        }
    }

    public enum AudioEnum
    {
        Music,
        Sound,
        Special,
    }
}