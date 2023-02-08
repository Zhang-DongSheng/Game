namespace FSM
{
    public interface ITransition
    {
        IState From { get; }

        IState To { get; }

        bool Condition();
    }
}