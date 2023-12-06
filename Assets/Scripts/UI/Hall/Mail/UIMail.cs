using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIMail : UIBase
    {
        [SerializeField] private PrefabTemplateBehaviour prefab;

        [SerializeField] private UIMailContent content;

        [SerializeField] private GameObject empty;

        private readonly List<ItemMail> items = new List<ItemMail>();

        public override void Refresh(UIParameter parameter)
        {
            var list = MailLogic.Instance.Mails;

            int count = Mathf.Min(list.Count, 99);

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    var item = prefab.Create<ItemMail>();
                    item.callback = OnClickMail;
                    items.Add(item);
                }
                items[i].Refresh(list[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
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
            content.Refresh(mail);
        }
    }
}
