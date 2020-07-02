using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
public class SimpleAnimation : MonoBehaviour
{
    private const float ANIMATION_SPEED = 6f;

    [SerializeField] private RectTransform target;

    [SerializeField] private CanvasGroup canvas;

    [SerializeField] private Vector3 positionOrigin;

    [SerializeField] private Vector3 positionDestination;

    [SerializeField] private Vector3 rotationOrigin;

    [SerializeField] private Vector3 rotationDestination;

    [SerializeField] private float scaleOrigin = 1;

    [SerializeField] private float scaleDestination = 1;

    [SerializeField] private float alphaOrigin = 1;

    [SerializeField] private float alphaDestination = 1;

    [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

    [SerializeField, Range(0.1f, 20)] private float speed = 0.1f;

    [SerializeField] private bool useConfig = true;

    public Action callBack;

    [SerializeField, Range(0, 1)] private float step;

    private float progress;

    private bool forward;

    private bool play;

    private void Awake()
    {
        if (target == null)
        {
            target = GetComponent<RectTransform>();
        }

        if (target != null && target is RectTransform)
        {
            if (alphaOrigin != alphaDestination && canvas == null)
            {
                canvas = target.gameObject.AddComponent<CanvasGroup>();
            }
        }

        speed = useConfig ? ANIMATION_SPEED : speed;
    }

    private void OnEnable()
    {
        Default(); Show();
    }

    private void OnDisable()
    {
        Default();
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

    public void Show(Action callBack = null)
    {
        this.callBack = callBack;

        SetActive(true);

        step = 0; forward = true; play = true;
    }

    public void Hide(Action callBack = null)
    {
        this.callBack = callBack;

        step = 0; forward = false; play = true;
    }

    private void Transition(float step)
    {
        progress = curve.Evaluate(step);

        target.anchoredPosition = Vector3.Lerp(positionOrigin, positionDestination, progress);

        target.localEulerAngles = Vector3.Lerp(rotationOrigin, rotationDestination, progress);

        target.localScale = Vector3.one * Mathf.Lerp(scaleOrigin, scaleDestination, progress);

        if (canvas != null) canvas.alpha = Mathf.Lerp(alphaOrigin, alphaDestination, progress);
    }

    private void Stop()
    {
        SetActive(forward);

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

    private void Default()
    {
        Transition(0);
    }

    public static SimpleAnimation AddCompontent(GameObject self, Transform target = null)
    {
        if (self == null) return null;

        SimpleAnimation animation = self.GetComponent<SimpleAnimation>();

        if (animation == null)
        {
            animation = self.AddComponent<SimpleAnimation>();
        }

        if (target == null)
        {
            target = self.transform.Find("Content");
        }
        animation.target = target != null ? target as RectTransform : self.transform as RectTransform;

        return animation;
    }

    public void ScaleAnimation(float origin = 0, float destination = 1)
    { 
        scaleOrigin = origin;
        scaleDestination = destination;
    }

    public void ShiftAnimation(Vector3 origin, Vector3 destination)
    {
        positionOrigin = origin;
        positionDestination = destination;
    }

    public void FadeAnimation(float origin = 0, float destination = 1)
    {
        alphaOrigin = origin;
        alphaDestination = destination;
    }
}