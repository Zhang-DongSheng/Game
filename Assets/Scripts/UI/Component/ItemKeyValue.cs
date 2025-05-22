using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemKeyValue : ItemBase
    {
        [SerializeField] private Text textKey;

        [SerializeField] private Text textValue;

        public void Refresh(string key, string value)
        {
            textKey.text = key;

            textValue.text = value;
        }
    }
}