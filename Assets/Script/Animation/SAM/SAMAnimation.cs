namespace UnityEngine.SAM
{
    [RequireComponent(typeof(Animation))]
    public class SAMAnimation : SAMBase
    {
        [SerializeField] private new Animation animation;

        private float time;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime;

                if (step >= time)
                {
                    step = Config.ZERO;

                    Completed();
                }
            }
        }

        protected override void Transition(float step) { }

        protected override void Compute()
        {
            status = Status.Compute;

            step = Config.ZERO;

            if (animation.clip == null) return;

            time = animation.clip.length;

            animation.Play();

            status = Status.Transition;
        }
    }
}