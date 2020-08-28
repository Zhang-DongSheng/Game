using UnityEngine;

public class SAMRotate : MonoBehaviour
{
    public Transform target;

    public Vector3 angle;

    public float speed;

    public bool rotate;

    private void Awake()
    {
        if (target == null)
            target = transform;
    }

    private void Update()
    {
        if (rotate)
        {
            target.Rotate(angle * speed * Time.deltaTime);
        }
    }
}