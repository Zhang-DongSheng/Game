using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FSM
{
    public class StateMachine : IStateMachine
    {
        protected string _name;

        protected bool _run;

        protected IState _default;

        protected IState _current;

        protected readonly Parameter _parameter = new Parameter();

        protected readonly List<IState> _states = new List<IState>();

        protected readonly List<ITransition> _transitions = new List<ITransition>();

        public string Name { get => _name; }

        public Parameter Parameter { get => _parameter; }

        public IState Default { get => _default; set => _default = value; }

        public IState Current { get => _current; set => _current = value; }

        public List<IState> States { get => _states; }

        public List<ITransition> Transitions { get => _transitions; }

        public StateMachine(string name)
        {
            _name = name;

            _run = false;
        }

        public void Startup()
        {
            if (_current == null)
            {
                _current = _default;
            }
            _run = true;
        }

        public void Update()
        {
            if (_run)
            {
                Any();

                Detection();

                if (_current != null)
                {
                    _current.OnStay();
                }
            }
        }

        public void Add(params IState[] states)
        {
            if (states == null) return;

            int count = states.Length;

            for (int i = 0; i < count; i++)
            {
                if (_states.Contains(states[i]))
                {
                    Debuger.LogWarning(Author.Script, "状态被重复添加");
                }
                else
                {
                    _states.Add(states[i]);
                }
            }
        }

        public void Remove(IState state)
        {
            if (_states.Contains(state))
            {
                _states.Remove(state);
            }
        }

        public void Add(params ITransition[] transitions)
        {
            if (transitions == null) return;

            int count = transitions.Length;

            for (int i = 0; i < count; i++)
            {
                if (_transitions.Contains(transitions[i]))
                {
                    Debuger.LogWarning(Author.Script, "状态被重复添加");
                }
                else
                {
                    _transitions.Add(transitions[i]);
                }
            }
        }

        public void Remove(ITransition transition)
        {
            if (_transitions.Contains(transition))
            {
                _transitions.Remove(transition);
            }
        }

        public void Switch(ITransition transition)
        {
            // 退出当前状态
            if (transition.From != null)
            {
                transition.From.OnExit();
            }
            else if (_current != null)
            {
                _current.OnExit();
            }
            // 更新状态
            _current = transition.To;
            // 进入下一个状态
            if (transition.To != null)
            {
                transition.To.OnEnter();
            }
        }

        public void Detection()
        {
            if (_current == null) return;

            for (int i = 0; i < _current.Transitions.Count; i++)
            {
                if (_current.Transitions[i].Condition())
                {
                    Switch(_current.Transitions[i]);
                    break;
                }
            }
        }

        public void Any()
        {
            for (int i = 0; i < _transitions.Count; i++)
            {
                if (_transitions[i].Condition())
                {
                    Switch(_transitions[i]);
                    break;
                }
            }
        }

        public IState Get(string name)
        {
            return _states.Find(x => x.Name.Equals(name));
        }
    }
}