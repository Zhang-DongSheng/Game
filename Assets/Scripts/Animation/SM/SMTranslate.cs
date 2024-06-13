using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 位移
    /// </summary>
    public class SMTranslate : SMBase
    {
        [SerializeField] private Vector3Interval interval;

        [SerializeField] private bool local;

        private Vector3 position;

        protected override void Initialize()
        {
            position = interval.origination;
        }

        protected override void Transition(float progress)
        {
            if (target == null) return;

            switch (circle)
            {
                case Circle.Loop:
                        position += interval.destination * Time.deltaTime * speed;
                    break;
                default:
                        position = interval.Lerp(progress);
                    break;
            }

            if (local)
            {
                if (target.TryGetComponent(out RectTransform rect))
                {
                    rect.anchoredPosition = position;
                }
                else
                {
                    target.localPosition = position;
                }
            }
            else
            {
                target.position = position;
            }
        }
    }
}