using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMail : UIBase
    {
        [SerializeField] private ListLayoutGroup layout;

        [SerializeField] private ItemMail prefab;

        [SerializeField] private UIMailContent content;

        [SerializeField] private GameObject empty;

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
            if (count > 0)
            {
                OnClickMail(list[0]);
            }
            else
            {
                content.SetActive(false);
            }
            SetActive(empty, count == 0);
        }

        private void OnClickMail(Mail mail)
        {
            select = mail.ID;

            content.Refresh(mail);

            layout.ForceUpdateContent();
        }
    }
}