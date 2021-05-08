namespace UnityEngine.UI
{
    public static class ScrollUtils
    {
        private static readonly float Min = 45f;

        private static readonly float Max = 135f;

        private static float angle;

        public static bool Horizontal(Vector2 vector)
        {
            angle = Vector2.Angle(vector, Vector2.up);

            return angle > Min && angle < Max;
        }

        public static bool Vertical(Vector2 vector)
        {
            angle = Vector2.Angle(vector, Vector2.up);

            return angle < Min || angle > Max;
        }
    }
}