using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RedDotLogic : Singleton<RedDotLogic>
    {
        private readonly Dictionary<RedKey, RedDot> _reddots = new Dictionary<RedKey, RedDot>();

        public void Init()
        {

        }

        public bool Trigger(params RedKey[] keys)
        {
            bool active = false;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] != RedKey.None)
                {
                    if (_reddots.ContainsKey(keys[i]) && _reddots[keys[i]].active)
                    {
                        active = true;
                        break;
                    }
                }
            }
            return active;
        }

        public void Update(RedKey key, bool value)
        {
            if (_reddots.ContainsKey(key))
            {
                _reddots[key].active = value;
            }
            else
            {
                _reddots.Add(key, new RedDot() { active = value });
            }
            EventManager.PostEvent(EventKey.RedDot, new EventMessageArgs());
        }

        public void Today(RedKey key)
        {
            string index = string.Format("red_{0}", key);

            if (LocalManager.GetInt(index) != DateTime.UtcNow.DayOfYear)
            {
                LocalManager.SetInt(index, DateTime.UtcNow.DayOfYear);

                Update(key, true);
            }
        }
    }

    public class RedDot
    {
        public bool active;
    }

    public enum RedKey
    {
        None,
        Test,
        Count,
    }
}