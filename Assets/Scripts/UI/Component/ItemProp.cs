using Game.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// µÀ¾ß
    /// </summary>
    public class ItemProp : ItemBase, IPointerClickHandler
    {
        public Action<uint> callback;

        [SerializeField] private TextBind txtName;

        [SerializeField] private TextBind txtNumber;

        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private ImageBind imgQuality;

        [SerializeField] private bool click;

        private PropInformation information;

        public void Refresh(Prop prop)
        {
            Refresh(prop.parallelism, prop.amount);
        }

        public void Refresh(uint propID, uint amount = 0)
        {
            information = DataProp.Get(propID);

            Refresh();

            txtNumber.SetText(amount);

            SetActive(true);
        }

        protected void Refresh()
        {
            if (information == null) return;

            txtName.SetText(information.name);

            imgIcon.SetSprite(information.icon);

            imgQuality.SetSprite(string.Format("quality_{0}", information.quality));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (click)
            {
                UIQuickEntry.Open(UIPanel.Introduce, new UIParameter()
                {
                    ["prop"] = information,
                });
            }
            else
            {
                callback?.Invoke(information.primary);
            }
        }
    }
}