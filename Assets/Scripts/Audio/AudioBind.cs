using UnityEngine;
using UnityEngine.UI;

namespace Game.Audio
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AudioBind : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audio;

        [SerializeField] private string url;

        public void Play()
        {
            m_audio.Play();
        }

        public void Stop()
        {
            m_audio.Stop();
        }
    }
}