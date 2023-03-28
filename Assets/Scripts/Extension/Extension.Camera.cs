 using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static Vector2 Size(this Camera camera, float distance = 0)
        {
            Vector2 size = new Vector2();

            if (camera == null) return size;

            if (camera.orthographic)
            {
                size.y = camera.orthographicSize * 2f;

                size.x = size.y * camera.aspect;
            }
            else
            {
                float fov = camera.fieldOfView * 0.5f * Mathf.Deg2Rad;

                size.y = distance * Mathf.Tan(fov) * -2f;

                size.x = size.y * camera.aspect;
            }
            return size;
        }

        public static float Zoom(this Camera camera, float value, float min = 0, float max = 180)
        {
            if (camera == null) return value;

            value = Mathf.Clamp(value, min, max);

            camera.fieldOfView = value;

            return value;
        }

        public static RenderTexture GetRenderTexture(this Camera camera, int width, int height)
        {
            if (camera.targetTexture == null)
            {
                camera.targetTexture = RenderTexture.GetTemporary(width, height, 0);

                RenderTexture.active = camera.targetTexture;
            }
            return camera.targetTexture;
        }

        public static void ReleaseRenderTexture(this Camera camera)
        {
            if (camera.targetTexture != null)
            {
                RenderTexture.ReleaseTemporary(camera.targetTexture);

                RenderTexture.active = null;
            }
            camera.targetTexture = null;
        }
    }
}