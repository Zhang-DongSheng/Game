namespace UnityEngine.SAM
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SAMCanvas : SAMBase
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private FloatInterval alpha;

        protected override void Awake()
        {
            if (canvas == null)
                canvas = GetComponent<CanvasGroup>();
        }

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(forward ? step : 1 - step);

                if (step >= Config.ONE)
                {
                    Completed();
                }
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