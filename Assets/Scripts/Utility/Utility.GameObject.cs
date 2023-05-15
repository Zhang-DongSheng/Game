namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// Unity GameObject
        /// </summary>
        public static class GameObject
        {
            public static bool IsNullOrEmpty(UnityEngine.GameObject target)
            {
                return System.Object.ReferenceEquals(target, null);
            }
        }
    }
}