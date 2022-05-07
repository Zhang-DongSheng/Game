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
            PropInformation info = DataManager.Instance.Load<DataProp>().Get((int)currency.currency);

            Refresh(info);

            TextHelper.SetString(m_prop.txtNumber, currency.number);

            SetActive(true);
        }

        public void Refresh(Prop prop)
        {
            PropInformation info = DataManager.Instance.Load<DataProp>().Get(prop.parallelism);

            Refresh(info);

            TextHelper.SetString(m_prop.txtNumber, prop.number);

            SetActive(true);
        }

        protected void Refresh(PropInformation prop)
        {
            if (prop != null)
            {
                SpriteHelper.SetSprite(m_prop.imgIcon, prop.icon);

                SpriteHelper.SetQuality(m_prop.imgQuality, prop.quality);

                TextHelper.SetString(m_prop.txtName, prop.name);
            }
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