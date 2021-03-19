using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMAlpha : SAMBase
    {
        [SerializeField] private FloatInterval alpha;

        private Color color;

        private readonly List<Graphic> graphics = new List<Graphic>();

        protected override void Awake()
        {
            base.Awake();

            graphics.Clear();

            graphics.AddRange(target.GetComponentsInChildren<Graphic>());
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