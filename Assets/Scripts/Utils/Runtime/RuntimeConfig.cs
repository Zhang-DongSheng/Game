namespace Game
{
    public static class RuntimeConfig
    {
        public const int Frame = 60;
    }

    public enum RuntimeEvent
    {
        FixedUpdate,
        Update,
        LateUpdate,
    }
}
