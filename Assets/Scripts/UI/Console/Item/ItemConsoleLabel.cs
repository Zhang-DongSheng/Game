using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemConsoleLabel : ItemBase
    {
        [SerializeField] private Text textKey;

        [SerializeField] private Text textValue;

        public void Refresh(string key, string value)
        {
            textKey.text = key;

            textValue.text = value;

            SetActive(true);
        }
    }
}