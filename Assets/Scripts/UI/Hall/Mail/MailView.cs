using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MailView : ViewBase
    {
        [SerializeField] private ListLayoutGroup layout;

        [SerializeField] private ItemMail prefab;

        [SerializeField] private Text m_title;

        [SerializeField] private Text m_content;

        [SerializeField] private ItemStatus m_status;

        private uint select;

        public override void Refresh(UIParameter parameter)
        {
            var list = MailLogic.Instance.Mails;

            int count = Mathf.Min(list.Count, 99);

            layout.SetData(prefab, list, (index, item, data) =>
            {
                item.Refresh(data, select, OnClickMail);
            });
            // Ìø×ª
            //if (count > 0)
            //{
            //    OnClickMail(list[0]);
            //}
            //else
            //{
            //    content.SetActive(false);
            //}
            //SetActive(empty, count == 0);
        }

        private void OnClickMail(Mail mail)
        {
            select = mail.ID;

            layout.ForceUpdateContent();
        }
    }
}