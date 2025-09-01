using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemReddot : ItemBase
    {
        [SerializeField] private GameObject target;

        [SerializeField] private List<int> list = new List<int>() { -1 };

        private bool active;

        protected override void OnRegister()
        {
            EventDispatcher.Register(UIEvent.Reddot, Refresh);
        }

        protected override void OnUnregister()
        {
            EventDispatcher.Unregister(UIEvent.Reddot, Refresh);
        }

        protected override void OnVisible(bool active)
        {
            if (active)
            {
                Refresh();
            }
        }

        protected virtual void Refresh(EventArgs args)
        {
            Refresh();
        }

        public void Refresh()
        {
            active = ReddotLogic.Instance.State(list.ToArray());

            SetActive(target, active);
        }

        public void Modify(params int[] keys)
        {
            list.Clear(); list.AddRange(keys);

            Refresh();
        }
    }
}