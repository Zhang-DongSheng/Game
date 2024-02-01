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

            m_menu.Refresh<Language>();

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
            var language = (Language)index;

            LanguageManager.Instance.Update(language);

            UIQuickEntry.OpenUINotice("ÓïÑÔ³É¹¦Îª" + language);
        }
    }
}