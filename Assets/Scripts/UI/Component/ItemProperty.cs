using UnityEngine;

namespace Game.UI
{
    public class ItemProperty : ItemBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private TextBind txtKey;

        [SerializeField] private TextBind txtValue;

        public void Refresh(int ID, float value, int type = 0)
        {

        }

        public void Refresh(string key, float value, int type = 0)
        {
            txtKey.SetText(key);

            txtValue.SetText(string.Format("{0}{1}", value, Unit(type)), false);
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