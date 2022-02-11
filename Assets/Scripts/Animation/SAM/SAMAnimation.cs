namespace UnityEngine.SAM
{
    [RequireComponent(typeof(Animation))]
    public class SAMAnimation : SAMBase
    {
        [SerializeField] private new Animation animation;

        private float interval;

        protected override void Init() { }

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime;

                if (step >= interval)
                {
                    step = Config.Zero;

                    Completed();
                }
            }
        }

        protected override void Transition(float step) { }

        protected override void Compute()
        {
            status = Status.Compute;

            step = Config.Zero;

            if (animation.clip == null) return;

            interval = animation.clip.length;

            animation.Play();

            status = Status.Transition;
        }
    }
}