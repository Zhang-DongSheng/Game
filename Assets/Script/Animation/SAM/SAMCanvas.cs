namespace UnityEngine.SAM
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SAMCanvas : SAMBase
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private float origin, destination;

        protected override void Awake()
        {
            if (canvas == null)
                canvas = GetComponent<CanvasGroup>();
        }

        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                step += speed * Time.deltaTime;

                Transition(forward ? step : 1 - step);

                if (step >= SAMConfig.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            if (canvas == null) return;

            progress = curve.Evaluate(step);

            canvas.alpha = Mathf.Lerp(origin, destination, progress);
        }
    }
}
