using UnityEngine.Events;

namespace UnityEngine.Polygon
{
    [RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
    public class PolygonSpriteButton : MonoBehaviour
    {
        [Space(5)] public UnityEvent onClick;

        private void OnMouseDown()
        {
            onClick?.Invoke();
        }
    }
}