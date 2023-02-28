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
    }
}