using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    ///  Ù–‘’π æ
    /// </summary>
    public class ItemProperty : ItemBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private TextBind txtKey;

        [SerializeField] private Text txtValue;

        public void Refresh(int ID, float value, int type = 0)
        {
            var key = ID.ToString();

            Refresh(key, ValueToString(type, value));
        }

        public void Refresh(string key, string value)
        {
            txtKey.SetText(key);

            txtValue.SetText(value);

            SetActive(true);
        }

        public static string ValueToString(int type, float value)
        {
            switch (type)
            {
                case 0:
                    return $"{value}";
                case 1:
                    return $"{(value * 100).Round(2)}%";
                case 2:
                    return $"{value.Round(2)}s";
                default:
                    return string.Empty;
            }
        }
    }
}