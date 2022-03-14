using UnityEngine;
using UnityEngine.UI;

namespace Game.SAM
{
    public class SAMImage : SAMBase
    {
        [SerializeField] private Image image;

        [SerializeField] private FloatInterval interval;

        protected override void Init()
        {
            if (image == null && !target.TryGetComponent(out image))
            {
                Debug.LogError("The Image is Missing!");
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