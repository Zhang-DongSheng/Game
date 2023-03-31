using UnityEngine;

namespace Game.Model
{
    public class ModelDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject self;

        [SerializeField] private Transform eye;

        [SerializeField] private CameraProjection projection;

        [SerializeField, Range(0.1f, 3)] private float speed = 1;

        private float zoom;

        public RenderTexture Texture { get { return projection.Texture; } }

        public void Rotate(float angle)
        {
            eye.Rotate(Vector3.up * speed * angle);
        }

        public void Zoom(float value)
        {
            zoom += value;

            zoom = Mathf.Clamp(zoom, 30, 100);

            projection.Zoom(zoom);
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