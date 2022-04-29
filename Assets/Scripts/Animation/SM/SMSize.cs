using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 改变大小
    /// </summary>
    public class SMSize : SMBase
    {
        [SerializeField] private Axis axis;

        [SerializeField] private Vector2Interval size;

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
            if (rect == null) return;

            progress = curve.Evaluate(step);

            vector = size.Lerp(progress);

            switch (axis)
            {
                case Axis.Horizontal:
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                    break;
                case Axis.Vertical:
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y);
                    break;
                default:
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                    goto case Axis.Vertical;
            }
        }
    }
}