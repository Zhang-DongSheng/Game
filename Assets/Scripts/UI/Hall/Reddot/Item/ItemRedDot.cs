using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemReddot : ItemBase
    {
        [SerializeField] private GameObject target;

        [SerializeField] private List<int> list = new List<int>() { -1 };

        private bool active;

        protected override void OnAwake()
        {
            EventManager.Register(EventKey.Reddot, Refresh);
        }

        protected override void OnRelease()
        {
            EventManager.Unregister(EventKey.Reddot, Refresh);
        }

        private void Refresh(EventMessageArgs args)
        {
            Refresh();
        }

        public void Refresh()
        {
            active = ReddotLogic.Instance.Trigger(list.ToArray());

            SetActive(target, active);
        }

        public void UpdeteRedDotKey(params int[] keys)
        {
            list.Clear();

            list.AddRange(keys); Refresh();
        }
    }
}