namespace UnityEngine.SAM
{
    public class SAMTranslate : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private Vector3Interval position;

        [SerializeField] private bool local = true;

        protected override void Transition(float step)
        {
            if (target == null) return;

            switch (circle)
            {
                case Circle.Always:
                    {
                        target.Translate(position.origin * Time.deltaTime * speed);
                    }
                    break;
                default:
                    {
                        progress = curve.Evaluate(step);

                        if (local)
                        {
                            target.anchoredPosition = position.Lerp(progress);
                        }
                        else
                        {
                            target.position = position.Lerp(progress);
                        }
                    }
                    break;
            }
        }

        protected override void Completed()
        {
            switch (circle)
            {
                case Circle.Single:
                    base.Completed();
                    break;
                default:
                    forward = !forward; step = Config.ZERO;
                    break;
            }
        }
    }
}