using UnityEngine.UI;

namespace UnityEngine.Audio
{
    [DisallowMultipleComponent]
    public class AudioHelper : MonoBehaviour
    {
        [SerializeField] private AudioTrigger trigger = AudioTrigger.None;

        [SerializeField] private string sound;

        private void Awake()
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