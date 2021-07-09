using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMClip : SAMBase
    {
        [SerializeField] Circle circle;

        [SerializeField] private Image image;

        [SerializeField] private List<Sprite> sprites;

        private IntInterval interval;

        private int index;

        protected override void Init()
        {
            if (image == null && !target.TryGetComponent(out image))
            {
                Debug.LogError("����ӹ���ͼƬ����");
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
            if (image == null) return;

            progress = curve.Evaluate(step);

            index = interval.Lerp(progress);

            if (sprites.Count > index)
            {
                image.sprite = sprites[index];
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