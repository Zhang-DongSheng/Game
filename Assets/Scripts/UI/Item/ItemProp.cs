using Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemProp : ItemBase, IPointerClickHandler
    {
        public Action<Prop> callback;

        [SerializeField] private UIPropBase m_prop;

        [SerializeField] private UIPropConfig m_config;

        public void Refresh(Currency currency)
        {
            PropInformation info = DataManager.Instance.Load<DataProp>().Get((int)currency.currency);

            Refresh(info);

            m_prop.txtNumber.SetText(currency.number);

            SetActive(true);
        }

        public void Refresh(Prop prop)
        {
            PropInformation info = DataManager.Instance.Load<DataProp>().Get(prop.parallelism);

            Refresh(info);

            m_prop.txtNumber.SetText(prop.number);

            SetActive(true);
        }

        protected void Refresh(PropInformation prop)
        {
            if (prop != null)
            {
                m_prop.txtName.SetText(prop.name);

                m_prop.imgIcon.SetSprite(prop.icon);

                m_prop.imgQuality.SetSprite(string.Format("quality_{0}", prop.quality));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_config.click)
            {
                UIQuickEntry.Open(UIPanel.UIIntroduce, new Paramter());
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