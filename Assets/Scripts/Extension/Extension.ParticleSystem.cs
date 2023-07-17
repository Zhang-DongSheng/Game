using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static void SetColor(this ParticleSystem particle, Color color)
        {
            ParticleSystem.MainModule main = particle.main;

            main.startColor = color;
        }

        public static void PlayParticle(this GameObject go)
        {
            if (go == null) return;

            ParticleSystem[] particles = go.GetComponentsInChildren<ParticleSystem>();

            int count = particles.Length;

            for (int i = 0; i < count; i++)
            {
                if (particles[i].isPlaying)
                {
                    particles[i].Stop();
                }
                particles[i].Play();
            }
        }
    }
}