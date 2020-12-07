using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
public class SAMSize : MonoBehaviour
{
    enum Axis
    {
        None,
        Horizontal,
        Vertical,
    }

    private const float ANIMATION_SPEED = 6f;

    [SerializeField] private RectTransform target;

    [SerializeField] private Axis axis;

    [SerializeField] private Vector2 origin;

    [SerializeField] private Vector2 destination;

    [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

    [SerializeField, Range(0.1f, 20)] private float speed = 0.1f;

    [SerializeField] private bool useConfig = true;

    [SerializeField, Range(0, 1)] private float step;

    private Vector2 current;

    private float progress;

    private bool forward;

    private bool play;

    public Action callBack;

    private void Awake()
    {
        if (target == null)
        {
            target = GetComponent<RectTransform>();
        }
        speed = useConfig ? ANIMATION_SPEED : speed;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            Transition(step);
        }
    }
#endif
    private void FixedUpdate()
    {
        if (play)
        {
            step += speed * Time.deltaTime;

            Transition(forward ? step : 1 - step);

            if (step > 1)
            {
                Stop();
            }
        }
    }

    private void Transition(float step)
    {
        progress = curve.Evaluate(step);

        current = Vector2.Lerp(origin, destination, progress);

        switch (axis)
        {
            case Axis.Horizontal:
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, current.x);
                break;
            case Axis.Vertical:
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, current.y);
                break;
            default:
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, current.x);

                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, current.y);
                break;
        }
    }

    private void Stop()
    {
        callBack?.Invoke();

        callBack = null;

        play = false; step = 0;
    }

    private void SetActive(bool active)
    {
        if (gameObject != null && gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }
    }

    public void Show()
    {
        SetActive(true);

        step = 0; forward = true; play = true;
    }

    public void Hide(Action callBack = null)
    {
        this.callBack = callBack;

        step = 0; forward = false; play = true;
    }

    public void Revise(Vector2 origin, Vector2 destination)
    {
        this.origin = origin;

        this.destination = destination;
    }

    public void Default()
    {
        Transition(0);
    }
}