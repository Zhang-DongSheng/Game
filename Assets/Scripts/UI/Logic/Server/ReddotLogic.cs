using Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic
{
    public class ReddotLogic : Singleton<ReddotLogic>, ILogic
    {
        private readonly List<Reddot> _reddots = new List<Reddot>();

        public void Initialize()
        {
            _reddots.Clear();
        }

        public Reddot Get(int key)
        {
            return _reddots.Find(x => x.key == key);
        }

        public void Set(int key, int value)
        {
            int index = _reddots.FindIndex(x => x.key == key);

            if (index > -1)
            {
                _reddots[key].value = value;
            }
            else
            {
                _reddots.Add(new Reddot(key, value));
            }
            EventDispatcher.Post(UIEvent.Reddot);
        }

        public bool State(params int[] keys)
        {
            bool active = false;

            for (int i = 0; i < keys.Length; i++)
            {
                var reddot = _reddots.Find(x => x.key == keys[i]);

                if (reddot != null && reddot.value > 0)
                {
                    active = true;
                    break;
                }
            }
            return active;
        }

        public void Clear(params int[] keys)
        {
            int count = keys.Length;

            if (count > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    int index = _reddots.FindIndex(x => x.key == keys[i]);

                    if (index > -1)
                    {
                        _reddots.RemoveAt(index);
                    }
                }
            }
            else
            {
                _reddots.Clear();
            }
            EventDispatcher.Post(UIEvent.Reddot);
        }
    }
}