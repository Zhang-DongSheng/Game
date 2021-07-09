using Game;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Image))]
    public class ImageFade : MonoBehaviour
    {
        [SerializeField] private Image image;

        [SerializeField, Range(0.1f, 6)] private float speed = 1;

        private Image background;

        private Sprite sprite;

        private Color color;

        private float step;

        private Status status;

        private void Awake()
        {
            if (image == null && !TryGetComponent(out image))
            {
                Debug.LogError("Image is Null!");
            }
            else
            {
                color = image.color;
            }
        }

        private void Update()
        {
            if (status == Status.Fade)
            {
                step += Time.deltaTime * speed;

                if (step >= 1)
                {
                    OnCompleted();
                }
                else
                {
                    OnValueChanged(step);
                }
            }
        }

        public void Fade(Sprite sprite)
        {
            status = Status.Ready;

            this.sprite = sprite;

            Background();

            background.sprite = sprite;

            color.a = 0;

            background.color = color;

            step = 0;

            status = Status.Fade;
        }

        public void Sprite(Sprite sprite, float alpha = 1)
        {
            status = Status.Ready;

            image.sprite = this.sprite = sprite;

            color.a = alpha;

            image.color = color;

            if (background != null)
            {
                background.SetActive(false);
            }
            status = Status.Idle;
        }

        private void OnValueChanged(float value)
        {
            color.a = value;

            background.color = color;

            color.a = 1f - value;

            image.color = color;
        }

        private void OnCompleted()
        {
            status = Status.Complete;

            float alpha = 1;

            image.sprite = sprite;

            color.a = alpha;

            image.color = color;

            background.SetActive(false);

            status = Status.Idle;
        }

        private void Background()
        {
            if (background == null)
            {
                GameObject go = new GameObject("background");

                RectTransform rect = go.AddComponent<RectTransform>();

                rect.SetParent(transform);

                rect.Full();

                background = go.AddComponent<Image>();
            }
            else
            {
                background.SetActive(true);
            }
        }

        enum Status
        {
            Idle,
            Ready,
            Fade,
            Complete,
        }
    }
}