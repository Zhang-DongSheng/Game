using System.Collections.Generic;

namespace FSM
{
    public interface IState
    {
        string Name { get; set; }

        List<ITransition> Transitions { get; }

        void OnEnter();

        void OnStay();

        void OnExit();
    }
}