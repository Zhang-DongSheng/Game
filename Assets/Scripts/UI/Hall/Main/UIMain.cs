using Game.State;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        [SerializeField] private List<ItemEntry> entries;

        [SerializeField] private List<ItemCurrency> currencies;

        protected override void OnRegister()
        {
            //EventManager.Register(EventKey.Currency)
        }

        protected override void OnUnregister()
        {
            
        }

        public override void Refresh(UIParameter parameter)
        {
            foreach (var entry in entries)
            {
                entry.Refresh();
            }
            RefreshCurrencies();

            //ModelLogic.Instance.Display((uint)Random.Range(1, 4));
        }

        private void RefreshCurrencies()
        {
            var list = new List<uint>() { 101, 102, 103 };

            int count = Mathf.Clamp(list.Count, 0, currencies.Count);

            for (int i = 0; i < count; i++)
            {
                currencies[i].Refresh(list[i]);
            }
            for (int i = count; i < currencies.Count; i++)
            {
                currencies[i].SetActive(false);
            }
        }
    }
}