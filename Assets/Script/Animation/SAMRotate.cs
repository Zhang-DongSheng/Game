namespace UnityEngine.SAM
{
    public class SAMRotate : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private Vector3 angle = new Vector3(0, 0, 1);

        [SerializeField, Range(0.1f, 100f)] private float speed = 1f;

        [SerializeField] private bool rotate = true;

        [SerializeField] private bool useConfig = true;

        private void Awake()
        {
            if (target == null)
                target = GetComponent<Transform>();

            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        private void Update()
        {
            if (rotate)
            {
                target.Rotate(angle * speed * Time.deltaTime);
            }
        }
    }
}