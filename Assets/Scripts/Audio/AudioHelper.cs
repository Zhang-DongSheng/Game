using UnityEngine;
using UnityEngine.UI;

namespace Game.Audio
{
    [DisallowMultipleComponent]
    public class AudioHelper : ItemBase
    {
        [SerializeField] private AudioTrigger trigger = AudioTrigger.None;

        [SerializeField] private string sound;

        protected override void OnAwake()
        {
            switch (trigger)
            {
                case AudioTrigger.Button:
                    {
                        if (TryGetComponent(out Button button))
                        {
                            button.onClick.AddListener(Play);
                        }
                    }
                    break;
                case AudioTrigger.Toggle:
                    {
                        if (TryGetComponent(out Toggle toggle))
                        {
                            toggle.onValueChanged.AddListener((isOn) =>
                            {
                                if (isOn)
                                {
                                    Play();
                                }
                            });
                        }
                    }
                    break;
                default: break;
            }
        }

        private void OnEnable()
        {
            if (trigger == AudioTrigger.Active)
            {
                Play();
            }
        }

        private void OnValidate()
        {
            
        }

        public void Play()
        {
            AudioManager.Instance.PlayEffect(sound);
        }

        enum AudioTrigger
        {
            None,
            Active,
            Button,
            Toggle,
        }
    }
}