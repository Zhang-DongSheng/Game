using UnityEngine;
using UnityEngine.UI;

namespace Game.SM
{
    /// <summary>
    /// 冷却
    /// </summary>
    public class SMCooling : SMBase
    {
        [SerializeField] private Image image;

        protected override void Init()
        {
            if (image == null && !target.TryGetComponent(out image))
            {
                Debuger.LogWarning(Author.UI, "The Image is Missing!");
            }
            else if (image.type != Image.Type.Filled)
            {
                Debuger.LogWarning(Author.UI, "The Image Type must be Filled!");
            }
            else
            {
                SetImageFillAmount(0);
            }
        }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(Config.ONE - step);

            SetImageFillAmount(progress);
        }

        private void SetImageFillAmount(float value)
        {
            if (image == null) return;

            image.fillAmount = value;

            image.raycastTarget = value > 0;
        }
    }
}