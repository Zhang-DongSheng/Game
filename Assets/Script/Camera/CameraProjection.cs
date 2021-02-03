using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraProjection : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        [SerializeField] private Vector2Int resolution = new Vector2Int(512, 512);

        public RenderTexture Texture { get; set; }

        private void Awake()
        {
            Texture = RenderTexture.GetTemporary(resolution.x, resolution.y, 1);

            if (camera == null)
                camera = GetComponent<Camera>();
            camera.targetTexture = Texture;
        }

        public void Zoom(float value)
        {
            camera.fieldOfView = value;
        }

        private void OnDestroy()
        {
            RenderTexture.ReleaseTemporary(Texture);
        }
    }
}