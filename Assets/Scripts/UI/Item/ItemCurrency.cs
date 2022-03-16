using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCurrency : ItemBase
    {
        [SerializeField] private Image imgIcon;

        [SerializeField] private Image imgQuality;

        [SerializeField] private Text txtName;

        [SerializeField] private Text txtNumber;

        public void Refresh(Currency currency)
        {
            CurrencyInformation info = DataHelper.Currency.Get(currency.currency);

            Factory.Instance.Pop("atlas_prop", (value) =>
            {
                SpriteAtlas atlas = value as SpriteAtlas;

                if (atlas && !string.IsNullOrEmpty(info.icon))
                {
                    imgQuality.sprite = atlas.GetSprite(info.icon);
                }
            });

            txtName.text = string.Format("{0}", info.name);

            txtNumber.text = string.Format("{0}", currency.number);

            SetActive(true);
        }
    }
}