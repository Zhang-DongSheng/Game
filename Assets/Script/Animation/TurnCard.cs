namespace UnityEngine.SAM
{
    public class TurnCard : MonoBehaviour
    {
        [SerializeField] private Transform card;

        [SerializeField] private GameObject front;

        [SerializeField] private GameObject back;

        [SerializeField] private float scale;

        [SerializeField] private float speed;

        private float destination;

        private float center;

        private float current;

        private float progress;

        private float size;

        private Status status;

        private void Awake()
        {
            if (card == null) card = transform;
        }

        private void Update()
        {
            if (status == Status.Transition)
            {
                current = Mathf.MoveTowards(current, destination, speed * Time.deltaTime);
                if (Mathf.Abs(current - destination) < 0.1f)
                {
                    current = destination;
                    status = Status.Idel;
                }
                Rotate(current);
            }
        }

        private void Rotate(float angle)
        {
            card.localEulerAngles = Vector3.up * angle;

            progress = 1 - Mathf.Abs(angle - center) / center;

            size = 1 + progress * (scale - 1);

            card.localScale = Vector3.one * size;

            SetSide(angle < center);
        }

        private void SetSide(bool side)
        {
            if (front != null && front.activeSelf != side)
                front.SetActive(side);
            if (back != null && back.activeSelf != !side)
                back.SetActive(!side);
        }

        public void ToFront()
        {
            current = card.localEulerAngles.y;

            destination = Config.ZERO;

            center = Config.Ninety;

            status = Status.Transition;
        }

        public void ToBack()
        {
            current = card.localEulerAngles.y;

            destination = Config.Ninety * 2;

            center = Config.Ninety;

            status = Status.Transition;
        }

        public void Default()
        {
            card.localEulerAngles = Vector3.zero;

            SetSide(true);

            status = Status.Idel;
        }
    }
}