using Data;
using Game.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISettingSound : UISettingBase
    {
        [SerializeField] private Slider musicVolume;

        [SerializeField] private Toggle musicMute;

        [SerializeField] private Slider soundVolume;

        [SerializeField] private Toggle soundMute;

        protected override void OnAwake()
        {
            musicVolume.value = GlobalVariables.Get<float>(string.Format("{0}_{1}", Const.AUDIO_VOLUME, AudioEnum.Music));

            musicMute.isOn = GlobalVariables.Get<bool>(string.Format("{0}_{1}", Const.AUDIO_Mute, AudioEnum.Music));

            soundVolume.value = GlobalVariables.Get<float>(string.Format("{0}_{1}", Const.AUDIO_VOLUME, AudioEnum.Sound));

            soundMute.isOn = GlobalVariables.Get<bool>(string.Format("{0}_{1}", Const.AUDIO_Mute, AudioEnum.Sound));

            musicVolume.onValueChanged.AddListener(OnValueChangedMusicVolume);

            musicMute.onValueChanged.AddListener(OnValueChangedMusicMute);

            soundVolume.onValueChanged.AddListener(OnValueChangedSoundVolume);

            soundMute.onValueChanged.AddListener(OnValueChangedSoundMute);
        }

        private void OnValueChangedMusicVolume(float value)
        {
            AudioManager.Instance.SetVolume(AudioEnum.Music, value);
        }

        private void OnValueChangedMusicMute(bool value)
        {
            AudioManager.Instance.SetMute(AudioEnum.Music, value);
        }

        private void OnValueChangedSoundVolume(float value)
        {
            AudioManager.Instance.SetVolume(AudioEnum.Sound, value);
        }

        private void OnValueChangedSoundMute(bool value)
        {
            AudioManager.Instance.SetMute(AudioEnum.Sound, value);
        }
    }
}