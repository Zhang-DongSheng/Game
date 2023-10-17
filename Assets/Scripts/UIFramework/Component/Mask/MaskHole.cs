namespace UnityEngine.UI
{
    public class MaskHole : Mask
    {
        public override bool IsRaycastLocationValid(Vector2 point, Camera camera)
        {
            if (!isActiveAndEnabled)
                return true;
            return !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, point, camera);
        }
    }
}