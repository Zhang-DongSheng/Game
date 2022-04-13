using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemProp : ItemBase
    {
        public Action<Prop> callback;

        [SerializeField] private UIPropBase m_prop;

        public void Refresh(Currency currency)
        {
            PropInformation info = DataManager.Instance.Load<DataCurrency>().Get((int)currency.currency);

            if (info != null)
            {
                SpriteManager.Instance.SetSprite(m_prop.imgQuality, info.icon);

                TextManager.Instance.SetString(m_prop.txtName, info.name);

                TextManager.Instance.SetString(m_prop.txtNumber, currency.number);
            }
            SetActive(true);
        }

        public void Refresh(Prop prop)
        {
            PropInformation info = DataManager.Instance.Load<DataProp>().Get(prop.parallelism);

            if (info != null)
            {
                SpriteManager.Instance.SetSprite(m_prop.imgQuality, info.icon);

                TextManager.Instance.SetString(m_prop.txtName, info.name);

                TextManager.Instance.SetString(m_prop.txtNumber, prop.number);
            }
            SetActive(true);
        }

        [System.Serializable]
        class UIPropBase
        {
            public Text txtName;

            public Text txtNumber;

            public Image imgIcon;

            public Image imgQuality;
        }
    }
}