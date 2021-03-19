using UnityEngine;

namespace Game
{
    public class ModelDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject self;

        [SerializeField] private Transform eye;

        [SerializeField] private new CameraProjection camera;

        [SerializeField, Range(0.1f, 3)] private float speed = 1;

        private float zoom;

        public RenderTexture Texture { get { return camera.Texture; } }

        public void Rotate(float angle)
        {
            eye.Rotate(Vector3.up * speed * angle);
        }

        public void Zoom(float value)
        {
            zoom += value;

            zoom.Between(30, 100);

            camera.Zoom(zoom);
        }

        public void Show()
        {
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }

        private void SetActive(bool active)
        {
            if (self != null && self.activeSelf != active)
            {
                self.SetActive(active);
            }
        }
    }
}