using Game.Audio;
using Game.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MainView : ViewBase
    {
        [SerializeField] private SubMainPlayer player;

        [SerializeField] private SubMainBanner banner;

        [SerializeField] private List<ItemCurrency> currencies;

        private readonly List<ItemEntry> entries = new List<ItemEntry>();

        protected override void OnAwake()
        {
            entries.Clear();

            entries.AddRange(transform.GetComponentsInChildren<ItemEntry>(true));
        }

        protected override void OnRegister()
        {
            
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

            ModelDisplayManager.Instance.SwitchGroup(1);

            ModelDisplayManager.Instance.RefreshModel(new ModelDisplayInformation()
            {
                path = "Package/Prefab/Model/Character/Female.prefab"
            });
            AudioManager.Instance.PlayMusic("HITA - ×íºìç¯");
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