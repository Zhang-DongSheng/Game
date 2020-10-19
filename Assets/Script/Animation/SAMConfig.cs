namespace UnityEngine.SAM
{
    public static class SAMConfig
    {
        public const int ZERO = 0;

        public const int ONE = 1;

        public const int Ninety = 90;

        public const float SPEED = 6f;
    }

    [System.Serializable]
    public class SAMInformation
    {
        public Vector3 position = Vector3.zero;

        public Vector3 rotation = Vector3.zero;

        public Vector3 scale = Vector3.one;

        public Color color = Color.white;

        public float alpha = 1f;
    }

    public enum SAMStatus
    { 
        Idel,
        Ready,
        Transition,
        Compute,
        Completed,
    }

    public enum SAMDirection
    { 
        Forward,
        Back,
    }
}