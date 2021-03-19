namespace UnityEngine.SAM
{
    public class SAMCanvas : SAMBase
    {
        [SerializeField] private FloatInterval alpha;

        private CanvasGroup canvas;

        protected override void Awake()
        {
            base.Awake();

            target.TryGetComponent<CanvasGroup>(out canvas);
        }

        protected override void Transition(float step)
        {
            if (canvas == null) return;

            progress = curve.Evaluate(step);

            canvas.alpha = alpha.Lerp(progress);
        }
    }
}