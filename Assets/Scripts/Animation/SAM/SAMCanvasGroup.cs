namespace UnityEngine.SAM
{
    public class SAMCanvasGroup : SAMBase
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private FloatInterval alpha;

        protected override void Init()
        {
            if (canvas == null && !target.TryGetComponent(out canvas))
            {
                canvas = gameObject.AddComponent<CanvasGroup>();
            }
        }

        protected override void Transition(float step)
        {
            if (canvas == null) return;

            progress = curve.Evaluate(step);

            canvas.alpha = alpha.Lerp(progress);
        }
    }
}