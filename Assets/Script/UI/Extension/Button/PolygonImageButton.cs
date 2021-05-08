using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// ¶à±ßÐÎ°´Å¥
    /// </summary>
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonImageButton : Image, IPointerClickHandler
    {
        private PolygonCollider2D polygon;

        [Space(5)] public UnityEvent onClick;

        protected override void Awake()
        {
            base.Awake();

            TryGetComponent<PolygonCollider2D>(out polygon);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out Vector3 point))
            {
                return polygon.OverlapPoint(point);
            }
            return false;
        }
    }
}