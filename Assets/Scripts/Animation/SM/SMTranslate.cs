using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 位移
    /// </summary>
    public class SMTranslate : SMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private bool anchor = true;

        [SerializeField] private Vector3Interval position;

        private RectTransform rect;

        protected override void Init()
        {
            if (!target.TryGetComponent(out rect))
            {
                Debuger.LogWarning(Author.UI, "The RectTransform is missing!");
            }
        }

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

                        if (anchor && rect != null)
                        {
                            rect.anchoredPosition = position.Lerp(progress);
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