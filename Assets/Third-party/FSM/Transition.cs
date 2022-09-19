using System;

namespace FSM
{
    public class Transition : ITransition
    {
        protected IState _from;

        protected IState _to;

        protected Func<bool> _match;

        public IState From { get => _from; }

        public IState To { get => _to; }

        public Transition(IState from, IState to, Func<bool> match)
        {
            _from = from;

            _to = to;

            _match = match;
        }

        public bool Condition()
        {
            if (_match != null)
            {
                return _match.Invoke();
            }
            return true;
        }
    }
}