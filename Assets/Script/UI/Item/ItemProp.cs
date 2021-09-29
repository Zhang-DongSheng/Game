using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemProp : ItemBase
    {
        [SerializeField] private Image imgIcon;

        [SerializeField] private Image imgQuality;

        [SerializeField] private Text txtName;

        [SerializeField] private Text txtNumber;

        public void Refresh(Currency currency)
        {
            CurrencyInformation info = DataHelper.Currency.Get(currency.currency);

            //_icon.sprite = info.icon;

            txtName.text = string.Format("{0}", info.name);

            txtNumber.text = string.Format("{0}", currency.number);

            SetActive(true);
        }

        public void Refresh(Prop prop)
        {
            PropInformation info = DataHelper.Prop.Get(prop.parallelism);

            //_icon.sprite = info.icon;

            txtName.text = string.Format("{0}", info.name);

            txtNumber.text = string.Format("{0}", prop.number);

            SetActive(true);
        }
    }
}