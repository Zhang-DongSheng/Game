using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static void PlayAnimator(this GameObject go, string name, State state = State.Play)
        {
            Animator animator = go.GetComponentInChildren<Animator>();

            if (animator != null)
            {
                switch (state)
                {
                    case State.Play:
                        animator.Play(name);
                        break;
                    case State.Pause:
                    case State.Stop:
                        animator.Play("");
                        break;
                }
            }
        }

        public static void PlayAnimation(this GameObject go, string name = null, State state = State.Play)
        {
            Animation animation = go.GetComponentInChildren<Animation>();

            if (animation != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = animation.clip.name;
                }
                switch (state)
                {
                    case State.Play:
                        {
                            if (animation.isPlaying && animation.IsPlaying(name)) { }
                            else
                            {
                                animation.Play(name);
                            }
                        }
                        break;
                    case State.Pause:
                    case State.Stop:
                        {
                            if (animation.isPlaying && animation.IsPlaying(name))
                            {
                                animation.Stop();
                            }
                        }
                        break;
                }
            }
        }

        public static void PlayEffect(this GameObject go, State state = State.Play)
        {
            ParticleSystem[] particles = go.GetComponentsInChildren<ParticleSystem>();

            for (int i = 0; i < particles.Length; i++)
            {
                switch (state)
                {
                    case State.Play:
                        particles[i].Play();
                        break;
                    case State.Pause:
                        particles[i].Pause();
                        break;
                    case State.Stop:
                        particles[i].Stop();
                        break;
                }
            }
        }
    }
}