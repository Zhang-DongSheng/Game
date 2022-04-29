namespace Game.SM
{
    public static class Config
    {
        public const int Zero = 0;

        public const int One = 1;

        public const int Ninety = 90;

        public const float Half = 0.5f;

        public const float Speed = 6f;
    }
    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        Idel,
        Ready,
        Transition,
        Pause,
        Compute,
        Completed,
    }
    /// <summary>
    /// 方向
    /// </summary>
    public enum Axis
    {
        None,
        Horizontal,
        Vertical,
    }
    /// <summary>
    /// 循环模式
    /// </summary>
    public enum Circle
    {
        Single,
        Always,
        Round,
    }
    /// <summary>
    /// 关联
    /// </summary>
    public enum Relevance
    {
        Self,
        Children,
    }
}