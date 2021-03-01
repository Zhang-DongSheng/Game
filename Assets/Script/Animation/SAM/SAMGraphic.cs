using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMGraphic : SAMBase
    {
        [SerializeField] private Graphic graphic;

        [SerializeField] private ColorInterval color;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(Format(forward, step));

                if (step >= Config.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            if (graphic == null) return;

            progress = curve.Evaluate(step);

            graphic.color = color.Lerp(progress);
        }
    }
}