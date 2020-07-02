using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float intensity = 0.3f;
    public float decay = 0.005f;

    private Vector3 fix_position;
    private Quaternion fix_rotation;
    private float shake_decay;
    private float shake_intensity;

    private void Awake()
    {
        fix_position = transform.position;
        fix_rotation = transform.rotation;
    }

    void Update()
    {
        if (shake_intensity > 0)
        {
            transform.position = fix_position + Random.insideUnitSphere * shake_intensity;
            transform.rotation = new Quaternion(
                fix_rotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                fix_rotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                fix_rotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                fix_rotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f);
            shake_intensity -= shake_decay;
        }
        else
        {
            transform.position = fix_position;
            transform.rotation = fix_rotation;
        }
    }

    [ContextMenu("StartUp")]
    public void StartUp()
    {
        shake_intensity = intensity;
        shake_decay = decay;
    }
}