using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMClip : SAMBase
    {
        [SerializeField] Circle circle;

        [SerializeField] private Image image;

        [SerializeField] private bool native;

        [SerializeField] private List<Sprite> sprites;

        private IntInterval interval;

        private int index, current = -1;

        protected override void Init()
        {
            if (image == null && !target.TryGetComponent(out image))
            {
                Debug.LogError("请添加关联图片引用");
            }
            else
            {
                interval = new IntInterval()
                {
                    origination = 0,
                    destination = sprites.Count
                };
            }
        }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            index = interval.Lerp(progress);

            if (sprites.Count > index && current != index)
            {
                current = index;

                if (image == null) return;

                image.sprite = sprites[current];

                if (native)
                {
                    image.SetNativeSize();
                }
            }
        }

        protected override void Completed()
        {
            switch (circle)
            {
                case Circle.Single:
                    base.Completed();
                    break;
                case Circle.Always:
                    step = Config.Zero;
                    break;
                default:
                    forward = !forward; step = Config.Zero;
                    break;
            }
        }
    }
}