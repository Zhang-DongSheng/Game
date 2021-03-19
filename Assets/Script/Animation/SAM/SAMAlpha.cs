using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMAlpha : SAMBase
    {
        [SerializeField] private Relevance relevance;

        [SerializeField] private FloatInterval alpha;

        private Color color;

        private readonly List<Graphic> graphics = new List<Graphic>();

        protected override void Awake()
        {
            base.Awake();

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