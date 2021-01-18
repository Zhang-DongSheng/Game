using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraProjection : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        [SerializeField] private RawImage image;

        [SerializeField] private Vector2Int resolution = new Vector2Int(512, 512);

        private RenderTexture texture;

        private void Awake()
        {
            texture = RenderTexture.GetTemporary(resolution.x, resolution.y, 1);

            if (camera == null)
                camera = GetComponent<Camera>();
            camera.targetTexture = texture;
        }

        private void Start()
        {
            if (image != null)
            {
                image.texture = texture;
            }
        }

        public void Zoom(float value)
        {
            camera.fieldOfView = value;
        }

        public void Rotate(GameObject target, Vector3 angle)
        {
            target.transform.localEulerAngles = angle;
        }

        private void OnDestroy()
        {
            RenderTexture.ReleaseTemporary(texture);
        }
    }
}