using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
public class SAMTouch : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Transform target;

    [SerializeField] private Graphic graphic;

    [SerializeField] private TouchType touchType;

    [SerializeField] private Vector3 positionOrigin;

    [SerializeField] private Vector3 positionDestination;

    [SerializeField] private Vector3 rotationOrigin;

    [SerializeField] private Vector3 rotationDestination;

    [SerializeField] private float scaleOrigin = 1;

    [SerializeField] private float scaleDestination = 1;

    [SerializeField] private Color colorOrigin = Color.white;

    [SerializeField] private Color colorDestination = Color.white;

    [SerializeField, Range(0.1f, 100)] private float speed = 0.1f;

    [SerializeField] private float time = 1;

    [SerializeField, Range(0, 1)] private float step;

    private bool forward;

    private bool play;

    private void Awake()
    {
        if (target == null)
            target = transform;
        if (graphic == null)
            graphic = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        Default();
    }

    private void Update()
    {
        if (play)
        {
            switch (touchType)
            {
                case TouchType.Click:
                    ComputeClick();
                    break;
                case TouchType.Press:
                case TouchType.Drag:
                    ComputePress();
                    break;
                case TouchType.Through:
                    ComputeThrough();
                    break;
            }

            if (Input.GetMouseButtonUp(0))
            {
                switch (touchType)
                {
                    case TouchType.Drag:
                        forward = false;
                        break;
                }
            }
        }
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (touchType)
        {
            case TouchType.Through:
                step = 0;
                forward = true;
                play = true;
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (touchType)
        {
            case TouchType.Press:
            case TouchType.Drag:
                step = 0;
                forward = true;
                play = true;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (touchType)
        {
            case TouchType.Click:
                step = 0;
                forward = true;
                play = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (touchType)
        {
            case TouchType.Press:
                forward = false;
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (touchType)
        {
            case TouchType.Through:
                forward = false;
                break;
        }
    }

    private void ComputeClick()
    {
        step += Time.deltaTime * speed * (forward ? 1 : -1);

        Transition(step);

        if (forward && step > time)
        {
            forward = false;
        }
        else if (!forward && 0 > step)
        {
            Stop();
        }
    }

    private void ComputePress()
    {
        step += Time.deltaTime * speed * (forward ? 1 : -1);

        step = step > 1 ? 1 : step;

        Transition(step);

        if (!forward && 0 > step)
        {
            Stop();
        }
    }

    private void ComputeThrough()
    {
        step += Time.deltaTime * speed * (forward ? 1 : -1);

        step = step > 1 ? 1 : step;

        Transition(step);

        if (!forward && 0 > step)
        {
            Stop();
        }
    }

    private void Transition(float progress)
    {
        if (graphic != null)
            graphic.color = Color.Lerp(colorOrigin, colorDestination, progress);

        target.localPosition = Vector3.Lerp(positionOrigin, positionDestination, progress);

        target.localEulerAngles = Vector3.Lerp(rotationOrigin, rotationDestination, progress);

        target.localScale = Vector3.one * Mathf.Lerp(scaleOrigin, scaleDestination, progress);
    }

    private void Stop()
    {
        play = false;

        step = 0;

        Transition(step);
    }

    private void Default()
    {
        Transition(0);
    }

    enum TouchType
    {
        None,
        Click,
        Press,
        Drag,
        Through,
    }
}