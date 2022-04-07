using Data;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemProp : ItemBase
    {
        public Action<Prop> callback;

        [SerializeField] private Image imgIcon;

        [SerializeField] private Image imgQuality;

        [SerializeField] private Text txtName;

        [SerializeField] private Text txtNumber;

        [SerializeField] private Button button;

        protected Prop prop;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(Prop prop)
        {
            this.prop = prop;

            PropInformation info = DataHelper.Prop.Get(prop.parallelism);

            Factory.Instance.Pop("atlas_prop", (value) =>
            {
                SpriteAtlas atlas = value as SpriteAtlas;

                if (atlas && !string.IsNullOrEmpty(info.icon))
                {
                    imgQuality.sprite = atlas.GetSprite(info.icon);
                }
            });

            txtName.text = string.Format("{0}", info.name);

            txtNumber.text = string.Format("{0}", prop.number);

            SetActive(true);
        }

        private void OnClick()
        {
            if (prop != null)
            {
                callback?.Invoke(prop);
            }
        }
    }
}