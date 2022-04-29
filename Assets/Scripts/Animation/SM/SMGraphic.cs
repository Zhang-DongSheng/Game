using UnityEngine;
using UnityEngine.UI;

namespace Game.SM
{
    /// <summary>
    /// 颜色
    /// </summary>
    public class SMGraphic : SMBase
    {
        [SerializeField] private Graphic graphic;

        [SerializeField] private ColorInterval color;

        protected override void Init()
        {
            if (graphic == null)
            {
                graphic = target.GetComponent<Graphic>();
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