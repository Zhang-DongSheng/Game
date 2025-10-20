using UnityEngine;
using UnityEngine.UI;

namespace Game.SM
{
    /// <summary>
    /// 冷却
    /// </summary>
    public class SMCooling : SMBase
    {
        [SerializeField] private FloatInterval interval;

        private Image image;

        protected override void Initialize()
        {
            image = target.GetComponent<Image>();

            if (image != null)
            {
                image.type = Image.Type.Filled;
            }
        }

        protected override void Transition(float progress)
        {
            if (image == null) return;

            float value = interval.Lerp(progress);

            image.fillAmount = value;

            image.raycastTarget = value > 0;
        }
    }
}