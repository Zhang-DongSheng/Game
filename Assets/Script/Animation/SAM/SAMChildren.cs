using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMChildren : SAMBase
    {
        [SerializeField] private List<Transform> children;

        protected override void Transition(float step)
        {
            if (children == null || children.Count == 0) return;

            progress = curve.Evaluate(step);

            for (int i = 0; i < children.Count; i++)
            {

            }
        }

        [ContextMenu("Children")]
        private void Children()
        {
            if (children == null) children = new List<Transform>();

            children.Clear();

            if (target != null && target.childCount > 0)
            {
                for (int i = 0; i < target.childCount; i++)
                {
                    children.Add(target.GetChild(i));
                }
            }
        }
    }
}