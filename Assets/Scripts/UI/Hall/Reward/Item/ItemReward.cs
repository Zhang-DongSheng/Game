using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemReward : ItemBase
    {
        [SerializeField] private ItemProp m_prop;

        public void Refresh(Prop prop)
        {
            m_prop.Refresh(prop);

            SetActive(true);
        }
    }
}