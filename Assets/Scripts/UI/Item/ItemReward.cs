using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemReward : MonoBehaviour
    {
        [SerializeField] private ItemProp prop;

        [SerializeField] private ItemCurrency currency;

        public void Refresh(Currency currency)
        { 
            
        }

        public void Refresh(Prop prop)
        {

        }
    }
}