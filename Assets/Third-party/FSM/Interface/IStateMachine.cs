using System.Collections.Generic;

namespace FSM
{
    public interface IStateMachine
    {
        string Name { get; }

        IState Default { get; set; }

        IState Current { get; set; }

        List<IState> States { get; }

        List<ITransition> Transitions { get; }

        void Add(params IState[] states);

        void Remove(IState state);

        void Add(params ITransition[] transitions);

        void Remove(ITransition transition);

        void Switch(ITransition transition);

        void Detection();

        void Any();

        IState Get(string name);
    }
}