using Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemProp : ItemBase, IPointerClickHandler
    {
        public Action<uint> callback;

        [SerializeField] private UIPropBase m_prop;

        [SerializeField] private UIPropConfig m_config;

        private PropInformation information;

        public void Refresh(Prop prop)
        {
            Refresh(prop.parallelism, prop.amount);
        }

        public void Refresh(RewardInformation reward)
        {
            Refresh(reward.propID, reward.amount);
        }

        public void Refresh(uint propID, int amount = 0)
        {
            information = DataProp.Get(propID);

            Refresh();

            m_prop.txtNumber.SetText(amount);

            SetActive(true);
        }

        protected void Refresh()
        {
            if (information == null) return;

            m_prop.txtName.SetText(information.name);

            m_prop.imgIcon.SetSprite(information.icon);

            m_prop.imgQuality.SetSprite(string.Format("quality_{0}", information.quality));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_config.click)
            {
                var parameter = new UIParameter();
                parameter.AddOrReplace("prop", information);
                UIQuickEntry.Open(UIPanel.UIIntroduce, parameter);
            }
            else
            {
                callback?.Invoke(information.primary);
            }
        }
        [System.Serializable]
        class UIPropBase
        {
            public TextBind txtName;

            public TextBind txtNumber;

            public ImageBind imgIcon;

            public ImageBind imgQuality;
        }
        [System.Serializable]
        class UIPropConfig
        {
            public bool click;
        }
    }
}