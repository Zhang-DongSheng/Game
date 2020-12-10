using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMGraphic : SAMBase
    {
        [SerializeField] private Graphic graphic;

        [SerializeField] private SAMGraphicInformation origin, destination;

        private Color color;

        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(step);

                if (step >= SAMConfig.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            if (graphic == null) return;

            progress = curve.Evaluate(step);

            color = Color.Lerp(origin.color, destination.color, progress);

            color.a = Mathf.Lerp(origin.alpha, destination.alpha, progress);

            graphic.color = color;
        }
    }

    [System.Serializable]
    public class SAMGraphicInformation
    {
        public Color color = Color.white;
        [Range(0, 1)]
        public float alpha = 1;
    }
}