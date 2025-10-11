using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraProjection : ItemBase
    {
        [SerializeField] protected Camera _camera;

        [SerializeField] protected Vector2 space = new Vector2(0.1f, 180);

        [SerializeField] protected Vector2Int resolution = new Vector2Int(512, 512);

        protected Vector2 size = new Vector2();

        public RenderTexture Texture { get; set; }

        protected override void OnAwake()
        {
            Texture = RenderTexture.GetTemporary(resolution.x, resolution.y, 1);

            if (_camera == null && !TryGetComponent(out _camera))
            {
                _camera = gameObject.AddComponent<Camera>();
            }
            _camera.targetTexture = Texture;
        }

        public void Zoom(float value)
        {
            if (_camera.orthographic)
            {
                _camera.orthographicSize = value;

                size.y = _camera.orthographicSize * 2f;

                size.x = size.y * Screen.width / Screen.height;
            }
            else
            {
                _camera.fieldOfView = Mathf.Clamp(value, space.x, space.y);
            }
        }

        public Vector2 Size
        {
            get
            {
                return size;
            }
        }

        protected override void OnRelease()
        {
            // 未注册函数调用，不需要调用基类
            RenderTexture.ReleaseTemporary(Texture);
        }
    }
}