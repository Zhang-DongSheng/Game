using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMGraphic : SAMBase
    {
        [SerializeField] private ColorInterval color;

        private Graphic graphic;

        protected override void Awake()
        {
            base.Awake();

            target.TryGetComponent<Graphic>(out graphic);
        }

        protected override void Transition(float step)
        {
            if (graphic == null) return;

            progress = curve.Evaluate(step);

            graphic.color = color.Lerp(progress);
        }
    }
}