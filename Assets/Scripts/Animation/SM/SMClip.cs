using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.SM
{
    /// <summary>
    /// 帧动画
    /// </summary>
    public class SMClip : SMBase
    {
        [SerializeField] private bool native;

        [SerializeField] private List<Sprite> sprites;

        private IntInterval interval;

        private Image image;

        private int index, current = -1;

        protected override void Initialize()
        {
            interval = new IntInterval()
            {
                start = 0,
                end = sprites.Count - 1
            };
            image = target.GetComponent<Image>();
        }

        protected override void Transition(float progress)
        {
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
    }
}