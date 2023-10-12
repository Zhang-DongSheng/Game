using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemEntry : ItemBase
    {
        [SerializeField] private UIPanel panel;

        [SerializeField] private Button button;

        [SerializeField] private GameObject mask;

        [SerializeField] private ItemReddot reddot;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public virtual void Refresh()
        {
            reddot.Refresh();

            SetActive(mask, false);
        }

        protected virtual void OnClick()
        {
            UIQuickEntry.Open(panel);
        }
    }
}
