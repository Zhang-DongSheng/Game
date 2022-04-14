using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCost : ItemBase
    {
        [SerializeField] private Image imgIcon;

        [SerializeField] private Text txtNumber;

        public void Refresh(Cost cost)
        {
            if (cost == null) return;

            switch (cost.consume)
            {
                default:
                    break;
            }
        }
    }
}