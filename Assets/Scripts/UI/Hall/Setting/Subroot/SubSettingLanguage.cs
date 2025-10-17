using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubSettingLanguage : SubviewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private Button button;

        private int index;

        protected override void OnAwake()
        {
            m_menu.Refresh<Language>();

            m_menu.callback = OnClickLanguage;

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

            UIQuickEntry.OpenNoticeView("语言成功为" + language);
        }
    }
}