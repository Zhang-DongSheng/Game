using Data;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ItemLanguage : ItemToggle
    {
        [SerializeField] private TextMeshProUGUI txtLabel;

        public override void Refresh(int index)
        {
            base.Refresh(index);

            Language language = (Language)index;

            txtLabel.text = language.ToString();
        }
    }
}
