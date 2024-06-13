namespace Game.SM
{
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
        Once,
        Loop,
        PingPong,
    }
}