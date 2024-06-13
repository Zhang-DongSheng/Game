using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.SM
{
    public class SMAlpha : SMBase
    {
        private CanvasGroup canvas;

        private readonly List<Graphic> graphics = new List<Graphic>();

        private float alpha;

        protected override void Initialize()
        {
            canvas = target.GetComponent<CanvasGroup>();

            graphics.Clear();

            graphics.AddRange(target.GetComponentsInChildren<Graphic>(true));
        }

        protected override void Transition(float progress)
        {
            alpha = progress;

            if (canvas != null)
            {
                canvas.alpha = alpha;
            }
            else
            {
                int count = graphics.Count;

                for (int i = 0; i < count; i++)
                {
                    var color = graphics[i].color;

                    color.a = alpha;

                    graphics[i].color = color;
                }
            }
        }
    }
}