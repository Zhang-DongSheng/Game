using UnityEngine.UI;

namespace UnityEngine.Audio
{
    [DisallowMultipleComponent]
    public class AudioHelper : MonoBehaviour
    {
        [SerializeField] private SoundTrigger trigger = SoundTrigger.None;

        [SerializeField] private string sound;

        private void Awake()
        {
            switch (trigger)
            {
                case SoundTrigger.Button:
                    {
                        if (GetComponent<Button>() is Button button)
                        {
                            button.onClick.AddListener(Play);
                        }
                    }
                    break;
                case SoundTrigger.Toggle:
                    {
                        if (GetComponent<Toggle>() is Toggle toggle)
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
            if (trigger == SoundTrigger.Active)
            {
                Play();
            }
        }

        public void Play()
        {
            AudioManager.Instance.PlayEffect(sound);
        }

        enum SoundTrigger
        {
            None,
            Active,
            Button,
            Toggle,
        }
    }
}