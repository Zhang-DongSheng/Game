using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ReddotLogic : Singleton<ReddotLogic>, ILogic
    {
        private readonly Dictionary<ReddotKey, Reddot> _reddots = new Dictionary<ReddotKey, Reddot>();

        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Reddot, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Reddot, OnReceivedInformation);
        }

        #region Function
        public bool Trigger(params ReddotKey[] keys)
        {
            bool active = false;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == ReddotKey.None) continue;

                if (_reddots.ContainsKey(keys[i]) && _reddots[keys[i]].active)
                {
                    active = true;
                    break;
                }
            }
            return active;
        }

        public void Update(ReddotKey key, bool value)
        {
            if (_reddots.ContainsKey(key))
            {
                _reddots[key].active = value;
            }
            else
            {
                _reddots.Add(key, new Reddot() { active = value });
            }
            EventManager.Post(EventKey.Reddot, new EventMessageArgs());
        }

        public void Today(ReddotKey key)
        {
            string index = string.Format("red_{0}", key);

            if (GlobalVariables.Get<int>(index) != DateTime.UtcNow.DayOfYear)
            {
                GlobalVariables.Set(index, DateTime.UtcNow.DayOfYear);

                Update(key, true);
            }
        }
        #endregion

        #region Request
        public void RequestInformation()
        {
            ScheduleLogic.Instance.Update(Schedule.Reddot);
        }
        #endregion

        #region Receive
        private void OnReceivedInformation(NetworkEventHandle handle)
        {

        }
        #endregion
    }

    public class Reddot
    {
        public bool active;
    }

    public enum ReddotKey
    {
        None,
        Test,
    }
}