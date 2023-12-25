using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISettingLanguage : UISettingBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private Button button;

        private int index;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickLanguage;

            var list = new List<int>();

            foreach (var value in Enum.GetValues(typeof(Language)))
            {
                list.Add((int)value);
            }
            m_menu.Refresh(list.ToArray());

            button.onClick.AddListener(OnClickConfirm);
        }

        public override void Refresh()
        {
            this.index = (int)LanguageManager.Instance.Language;

            m_menu.Select(index);
        }

        private void OnClickLanguage(int index)
        {
            this.index = index;
        }

        private void OnClickConfirm()
        {
            LanguageManager.Instance.Refresh((Language)index);
        }
    }
}