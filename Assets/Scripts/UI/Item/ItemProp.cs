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

        [SerializeField] private Button m_button;

        protected Prop prop;

        private void Awake()
        {
            m_button.onClick.AddListener(OnClick);
        }

        public void Refresh(Prop prop)
        {
            this.prop = prop;

            PropInformation info = WarehouseLogic.PropInformation(prop.parallelism);

            if (info != null)
            {
                SpriteManager.Instance.SetSprite(m_prop.imgQuality, info.icon);

                TextManager.Instance.SetString(m_prop.txtName, info.name);

                TextManager.Instance.SetString(m_prop.txtNumber, prop.number);
            }
            SetActive(true);
        }

        private void OnClick()
        {
            if (prop != null)
            {
                callback?.Invoke(prop);
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