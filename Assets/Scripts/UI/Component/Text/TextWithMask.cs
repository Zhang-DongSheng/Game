namespace UnityEngine.UI
{
    public class TextWithMask : MonoBehaviour
    {
        [SerializeField] private Text component;

        [SerializeField] private RectTransform mask;

        [SerializeField] private float speed = 1f;

        [SerializeField] private float time = 1f;

        [SerializeField] private float offset;

        private RectTransform target;

        private Vector2 position;

        private Status status;

        private float step;

        private bool overflow;

        private string content = string.Empty;

        private readonly float[] space = new float[3] { 0, 0, 0 };

        private void Awake()
        {
            if (mask != null || TryGetComponent(out mask))
            {
                space[0] = mask.rect.width;
            }

            if (component == null) component = GetComponentInChildren<Text>();

            if (component != null)
            {
                target = component.rectTransform;

                target.pivot = new Vector2(0, 0.5f);

                target.anchorMin = target.anchorMax = new Vector2(0, 0.5f);

                position = target.anchoredPosition;
            }
        }

        private void Update()
        {
            if (component != null && component.text != content)
            {
                Recalculate(component.text);
            }
            if (status == Status.Fixed) return;

            switch (status)
            {
                case Status.Hover:
                    {
                        step += Time.deltaTime;

                        if (step > time)
                        {
                            status = Status.Through;
                        }
                    }
                    break;
                case Status.Through:
                    {
                        position.x -= Time.deltaTime * speed;

                        if (position.x < -space[2])
                        {
                            status = Status.Finish;
                        }
                    }
                    break;
                case Status.Finish:
                    {
                        step = 0;

                        position.x = 0;

                        status = Status.Hover;
                    }
                    break;
            }
            target.anchoredPosition = position;
        }

        private void Recalculate(string value)
        {
            component.text = content = value;

            overflow = component.preferredWidth > space[0];

            space[1] = overflow ? component.preferredWidth : space[0];

            space[2] = overflow ? space[1] - space[0] + offset : space[0];

            status = overflow ? Status.Hover : Status.Fixed;

            step = 0;

            component.alignment = overflow ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, space[1]);

            target.anchoredPosition = position = new Vector2(0, position.y);
        }

        enum Status
        {
            Fixed,
            Hover,
            Through,
            Finish,
        }
    }
}