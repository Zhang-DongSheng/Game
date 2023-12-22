using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemActivityTab : ItemToggle
    {
        [SerializeField] private Text[] labels;

        public override void Refresh(int index)
        {
            var table = DataActivity.Get((uint)index);

            if (table != null)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].text = table.name;
                }
            }
            else
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].text = index.ToString();
                }
            }
            base.Refresh(index);
        }
    }
}