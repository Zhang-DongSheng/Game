using UnityEngine;
using UnityEngine.Events;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioVisualizer : ItemBase
    {
        [SerializeField] private AudioSource source;

        [SerializeField] private int count;

        private readonly float[] samples = new float[256];

        public UnityEvent<int, float> onValueChanged;

        protected override void OnAwake()
        {
            if (source != null || TryGetComponent(out source))
            {
                source.Play();
            }
            else
            {
                Debuger.LogError(Author.Resource, "can't find audiosource!");
            }
        }

        protected override void OnUpdate(float delta)
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