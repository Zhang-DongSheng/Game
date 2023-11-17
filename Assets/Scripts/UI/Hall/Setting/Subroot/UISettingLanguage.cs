using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISettingLanguage : UISettingBase
    {
        [SerializeField] private PrefabTemplate prefab;

        [SerializeField] private Button btnConfirm;

        private int index;

        private readonly List<ItemLanguage> items = new List<ItemLanguage>();

        protected override void OnAwake()
        {
            btnConfirm.onClick.AddListener(OnClickConfirm);
        }

        public override void Refresh()
        {
            int count, index = 0;

            foreach (var language in Enum.GetValues(typeof(Language)))
            {
                if (index >= items.Count)
                {
                    var item = prefab.Create<ItemLanguage>();
                    item.callback = OnClickLanguage;
                    items.Add(item);
                }
                items[index++].Refresh((int)language);
            }
            this.index = (int)LanguageManager.Instance.Language;

            count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[i].Select(this.index);
            }
        }

        private void OnClickLanguage(int index)
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[i].Select(index);
            }
            this.index = index;
        }

        private void OnClickConfirm()
        {
            LanguageManager.Instance.Refresh((Language)index);
        }
    }
}