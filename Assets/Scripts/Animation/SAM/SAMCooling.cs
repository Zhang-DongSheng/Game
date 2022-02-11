using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMCooling : SAMBase
    {
        [SerializeField] private Image image;

        protected override void Init()
        {
            if (image == null && !target.TryGetComponent(out image))
            {
                Debug.LogError("请添加关联图片引用");
            }
            else
            {
                SetImageFillAmount(0);
            }
        }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(Config.One - step);

            SetImageFillAmount(progress);
        }

        private void SetImageFillAmount(float value)
        {
            if (image == null) return;

            image.raycastTarget = value > 0;

            image.fillAmount = value;
        }
    }
}