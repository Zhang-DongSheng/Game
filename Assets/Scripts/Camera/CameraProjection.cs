using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraProjection : MonoBehaviour
    {
        [SerializeField] protected new Camera camera;

        [SerializeField] protected Vector2 space = new Vector2(0.1f, 180);

        [SerializeField] protected Vector2Int resolution = new Vector2Int(512, 512);

        protected Vector2 size = new Vector2();

        public RenderTexture Texture { get; set; }

        private void Awake()
        {
            Texture = RenderTexture.GetTemporary(resolution.x, resolution.y, 1);

            if (camera == null && !TryGetComponent(out camera))
            {
                camera = gameObject.AddComponent<Camera>();
            }
            camera.targetTexture = Texture;
        }

        public void Zoom(float value)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize = value;

                size.y = camera.orthographicSize * 2f;

                size.x = size.y * Screen.width / Screen.height;
            }
            else
            {
                camera.fieldOfView = Mathf.Clamp(value, space.x, space.y);
            }
        }

        public Vector2 Size
        {
            get
            {
                return size;
            }
        }

        private void OnDestroy()
        {
            RenderTexture.ReleaseTemporary(Texture);
        }
    }
}