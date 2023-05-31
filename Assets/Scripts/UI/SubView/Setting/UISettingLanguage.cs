using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UISettingLanguage : UISettingBase
    {
        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemLanguage> items = new List<ItemLanguage>();

        public override void Refresh()
        {
            int count, index = 0;

            foreach (var language in Enum.GetValues(typeof(Data.Language)))
            {
                if (index >= items.Count)
                {
                    var item = prefab.Create<ItemLanguage>();
                    item.callback = OnClick;
                    items.Add(item);
                }
                items[index].Refresh((int)language);
            }
            count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[index].Select((int)LanguageManager.Instance.Current);
            }
        }

        private void OnClick(int index)
        { 
            
        }
    }
}