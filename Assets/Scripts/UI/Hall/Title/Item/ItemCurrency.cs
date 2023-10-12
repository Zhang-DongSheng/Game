using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCurrency : ItemBase
    {
        [SerializeField] private TextMeshProUGUI value;

        [SerializeField] private Button source;

        public void Refresh()
        { 
            
        }
    }
}