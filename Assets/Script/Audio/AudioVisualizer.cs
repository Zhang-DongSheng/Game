using UnityEngine.Events;

namespace UnityEngine.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioVisualizer : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        [SerializeField] private int count;

        private readonly float[] samples = new float[256];

        public UnityEvent<int, float> onValueChanged;

        private void Awake()
        {
            if (source != null || TryGetComponent(out source))
            {
                source.Play();
            }
            else
            {
                Debug.LogError("can't find audiosource!");
            }
        }

        void Update()
        {
            if (source.isPlaying)
            {
                Visulization();
            }
        }

        private void Visulization()
        {
            source.GetSpectrumData(samples, 0, FFTWindow.Triangle);

            for (int i = 0; i < count; i++)
            {
                onValueChanged?.Invoke(i, samples[i]);
            }
        }
    }
}