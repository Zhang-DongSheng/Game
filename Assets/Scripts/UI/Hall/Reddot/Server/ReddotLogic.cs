using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ReddotLogic : Singleton<ReddotLogic>, ILogic
    {
        private readonly List<Reddot> _reddots = new List<Reddot>();

        public void Initialize()
        {
            NetworkEventManager.Register(NetworkEventKey.Reddot, OnReceivedInformation);
        }

        public void Release()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Reddot, OnReceivedInformation);
        }

        public bool Trigger(params int[] keys)
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

        public void Update(int key, int value)
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
            EventManager.Post(EventKey.Reddot, new EventMessageArgs());
        }

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
        public int key;

        public int type;

        public int value;

        public Reddot(int key, int value)
        {
            this.key = key;

            this.value = value;
        }
    }
}