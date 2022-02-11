namespace UnityEngine.SAM
{
    public class SAMScale : SAMBase
    {
        [SerializeField] private Vector3Interval scale;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            vector = scale.Lerp(progress);

            target.localScale = vector;
        }
    }
}