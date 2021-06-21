using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemProp : ItemBase
    {
        public void Refresh(Prop prop)
        {
            DataProp data = DataManager.Instance.Load<DataProp>("Prop", "Data/Prop");


        }
    }
}
