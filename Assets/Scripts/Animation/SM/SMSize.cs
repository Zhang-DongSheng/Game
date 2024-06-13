using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 改变大小
    /// </summary>
    public class SMSize : SMBase
    {
        [SerializeField] private Vector2Interval interval;

        [SerializeField] private Axis axis;

        private Vector2 size;

        private RectTransform rect;

        protected override void Initialize()
        {
            rect = target.GetComponent<RectTransform>();
        }

        protected override void Transition(float progress)
        {
            if (rect == null) return;

            size = interval.Lerp(progress);

            switch (axis)
            {
                case Axis.Horizontal:
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    break;
                case Axis.Vertical:
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                    break;
                default:
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    goto case Axis.Vertical;
            }
        }
    }
}