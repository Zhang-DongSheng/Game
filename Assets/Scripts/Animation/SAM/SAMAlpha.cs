using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.SAM
{
    public class SAMAlpha : SAMBase
    {
        [SerializeField] private Relevance relevance;

        [SerializeField] private FloatInterval alpha;

        private readonly List<Graphic> graphics = new List<Graphic>();

        private Color color;

        protected override void Init()
        {
            graphics.Clear();

            switch (relevance)
            {
                case Relevance.Self:
                    if (target.TryGetComponent(out Graphic graphic))
                    {
                        graphics.Add(graphic);
                    }
                    break;
                case Relevance.Children:
                    graphics.AddRange(target.GetComponentsInChildren<Graphic>());
                    break;
            }
        }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            for (int i = 0; i < graphics.Count; i++)
            {
                color = graphics[i].color;
                color.a = alpha.Lerp(progress);
                graphics[i].color = color;
            }
        }
    }
}