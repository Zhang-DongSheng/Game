using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemProperty : ItemBase
    {
        [SerializeField] private Image icon;

        [SerializeField] private Text key;

        [SerializeField] private Text value;

        public void Refresh(int ID, float value, int type = 0)
        {
            
        }

        public void Refresh(string key, float value, int type = 0)
        {
            TextHelper.SetString(this.key, key);

            this.value.text = string.Format("{0}{1}", value, Unit(type));
        }

        public static string Unit(int type)
        {
            switch (type)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return "%";
                case 2:
                    return "s";
                default:
                    return string.Empty;
            }
        }
    }
}