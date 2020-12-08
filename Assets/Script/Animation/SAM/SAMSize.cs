using UnityEngine;
using UnityEngine.SAM;

public class SAMSize : SAMBase
{
    [SerializeField] private SAMAxis axis;

    private bool forward;

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
        if (target == null) return;

        progress = curve.Evaluate(step);

        vector = Vector2.Lerp(origin.size, destination.size, progress);

        switch (axis)
        {
            case SAMAxis.Horizontal:
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                break;
            case SAMAxis.Vertical:
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y);
                break;
            default:
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y);
                break;
        }
    }

    protected override void Completed()
    {
        status = SAMStatus.Completed;

        onCompleted?.Invoke();
    }

    protected override void Compute()
    {
        status = SAMStatus.Compute;

        step = 0;

        onBegin?.Invoke();

        status = SAMStatus.Transition;
    }

    public override void Begin(bool forward)
    {
        this.forward = forward;

        Compute();
    }
}