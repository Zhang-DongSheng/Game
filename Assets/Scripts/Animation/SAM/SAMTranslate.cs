using UnityEngine;

namespace Game.SAM
{
    public class SAMTranslate : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private bool anchor = true;

        [SerializeField] private Vector3Interval position;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            if (target == null) return;

            switch (circle)
            {
                case Circle.Always:
                    {
                        target.Translate(position.origination * Time.deltaTime * speed);
                    }
                    break;
                default:
                    {
                        progress = curve.Evaluate(step);

                        if (anchor)
                        {
                            target.anchoredPosition = position.Lerp(progress);
                        }
                        else
                        {
                            target.localPosition = position.Lerp(progress);
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
                    forward = !forward; step = Config.Zero;
                    break;
            }
        }
    }
}