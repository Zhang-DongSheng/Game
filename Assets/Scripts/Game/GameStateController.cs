using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.State
{
    public sealed class GameStateController : Singleton<GameStateController>
    {
        private readonly List<IGameState> _states = new List<IGameState>();

        public IGameState current { get; private set; }

        public void Init()
        {
            EnterState<GameSplashState>();
        }

        public void EnterState<T>() where T : IGameState
        {
            if (current != null)
            {
                if (current.GetType() == typeof(T)) return;

                current.OnExit();

                current = GetState<T>();

                current.OnEnter();
            }
            else
            {
                current = GetState<T>();

                current.OnEnter();
            }
        }

        private IGameState GetState<T>() where T : IGameState
        {
            foreach (var state in _states)
            {
                if (_states.GetType().Equals(typeof(T)))
                {
                    return state;
                }
            }
            try
            {
                var state = Activator.CreateInstance(typeof(T)) as IGameState;

                state.OnCreate(); _states.Add(state);

                return state;
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.Script, e);
            }
            return null;
        }
    }
}