using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemReddot : ItemBase
    {
        [SerializeField] private GameObject target;

        [SerializeField] private ReddotKey main;

        [SerializeField] private List<ReddotKey> list;

        private bool active;

        protected override void OnAwake()
        {
            EventManager.Register(EventKey.Reddot, Refresh);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventManager.Unregister(EventKey.Reddot, Refresh);
        }

        private void Start()
        {
            Refresh();
        }

        private void Refresh(EventMessageArgs args)
        {
            Refresh();
        }

        private void Refresh()
        {
            active = ReddotLogic.Instance.Trigger(main) || ReddotLogic.Instance.Trigger(list.ToArray());

            SetActive(target, active);
        }

        public void UpdeteRedDotKey(params ReddotKey[] keys)
        {
            main = ReddotKey.None; list.Clear();

            switch (keys.Length)
            {
                case 0:
                    break;
                case 1:
                    main = keys[0];
                    break;
                default:
                    list.AddRange(keys);
                    break;
            }
            Refresh();
        }
    }
}