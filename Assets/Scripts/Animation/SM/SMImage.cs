using UnityEngine;
using UnityEngine.UI;

namespace Game.SM
{
    public class SMImage : SMBase
    {
        [SerializeField] private Image image;

        [SerializeField] private FloatInterval interval;

        protected override void Init()
        {
            if (image == null && !target.TryGetComponent(out image))
            {
                Debuger.LogWarning(Author.UI, "The Image is missing!");
            }
        }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            SetImageFillAmount(interval.Lerp(progress));
        }

        private void SetImageFillAmount(float value)
        {
            if (image == null) return;

            image.fillAmount = value;
        }

        public void Begin(float start, float end)
        {
            interval.origination = start;

            interval.destination = end;

            Begin(true);
        }
    }
}