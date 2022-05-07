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

            if (info != null)
            {
                SpriteHelper.SetSprite(m_prop.imgIcon, info.icon);

                SpriteHelper.SetQuality(m_prop.imgQuality, info.quality);

                TextHelper.SetString(m_prop.txtName, info.name);

                TextHelper.SetString(m_prop.txtNumber, currency.number);
            }
            SetActive(true);
        }

        public void Refresh(Prop prop)
        {
            PropInformation info = DataManager.Instance.Load<DataProp>().Get(prop.parallelism);

            if (info != null)
            {
                SpriteHelper.SetSprite(m_prop.imgIcon, info.icon);

                SpriteHelper.SetQuality(m_prop.imgQuality, info.quality);

                TextHelper.SetString(m_prop.txtName, info.name);

                TextHelper.SetString(m_prop.txtNumber, prop.number);
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