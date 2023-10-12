using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ItemProp : ItemBase, IPointerClickHandler
    {
        [SerializeField] private UIPropBase m_prop;

        [SerializeField] private UIPropConfig m_config;

        private PropInformation information;

        public void Refresh(Currency currency)
        {
            information = DataManager.Instance.Load<DataProp>().Get((int)currency.currency);

            Refresh(information);

            m_prop.txtNumber.SetText(currency.number);

            SetActive(true);
        }

        public void Refresh(Prop prop)
        {
            information = DataManager.Instance.Load<DataProp>().Get(prop.parallelism);

            Refresh(information);

            m_prop.txtNumber.SetText(prop.number);

            SetActive(true);
        }

        protected void Refresh(PropInformation prop)
        {
            if (prop != null)
            {
                m_prop.txtName.SetText(prop.name);

                m_prop.imgIcon.SetSprite(prop.icon);

                m_prop.imgQuality.SetSprite(string.Format("quality_{0}", (int)prop.quality));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_config.click)
            {
                var parameter = new UIParameter();
                parameter.AddOrReplace("prop", information);
                UIQuickEntry.Open(UIPanel.UIIntroduce, parameter);
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