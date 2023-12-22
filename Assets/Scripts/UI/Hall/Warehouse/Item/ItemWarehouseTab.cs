using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemWarehouseTab : ItemToggle
    {
        [SerializeField] private Text[] labels;

        public override void Refresh(int index)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].text = index.ToString();
            }
            base.Refresh(index);
        }
    }
}
