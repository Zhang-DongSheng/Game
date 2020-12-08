namespace UnityEngine.SAM
{
    public static class SAMConfig
    {
        public const int ZERO = 0;

        public const int ONE = 1;

        public const int Ninety = 90;

        public const float SPEED = 6f;
    }

    public enum SAMStatus
    { 
        Idel,
        Ready,
        Transition,
        Compute,
        Completed,
    }

    public enum SAMAxis
    {
        None,
        Horizontal,
        Vertical,
    }

    public enum SAMDirection
    { 
        Forward,
        Back,
    }

    public enum SAMCircle
    { 
        Once,
        Loop,
    }
}