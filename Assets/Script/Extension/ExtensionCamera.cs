using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static Vector2 Size(this Camera camera, float distance = 0)
        {
            if (camera == null) return Vector2.zero;

            Vector2 size = new Vector2();

            if (camera.orthographic)
            {
                size.y = camera.orthographicSize * 2f;
                size.x = size.y * Screen.width / Screen.height;
            }
            else
            {
                size.y = distance * Mathf.Tan(camera.fieldOfView * 0.5f) * 2f;
                size.x = size.y * Screen.width / Screen.height;
            }
            return size;
        }
    }
}