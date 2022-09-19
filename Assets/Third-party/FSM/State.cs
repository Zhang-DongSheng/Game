using System;
using System.Collections.Generic;

namespace FSM
{
    public class State : IState
    {
        protected string _name;

        protected readonly List<ITransition> _transitions = new List<ITransition>();

        public string Name { get => _name; set => _name = value; }

        public List<ITransition> Transitions { get => _transitions; }

        public Action onEnter, onStay, onExit;

        public State(string name)
        {
            _name = name;
        }

        public virtual void OnEnter()
        {
            onEnter?.Invoke();
        }

        public virtual void OnStay()
        {
            onStay?.Invoke();
        }

        public virtual void OnExit()
        {
            onExit?.Invoke();
        }

        public void Add(ITransition transition)
        {
            if (_transitions != null && !_transitions.Contains(transition))
            {
                _transitions.Add(transition);
            }
        }

        public void Remove(ITransition transition)
        {
            if (_transitions != null && _transitions.Contains(transition))
            {
                _transitions.Remove(transition);
            }
        }
    }
}