using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIWarehouse : UIBase
    {
        [SerializeField] private ItemTabGroup tab;

        private void Awake()
        {
            tab.callback = OnClickTab;
        }

        private void Start()
        {
            tab.Initialize(10);
        }

        public void Refresh()
        {

        }

        private void OnClickTab(int index)
        {
            Debug.LogError(index);
        }
    }
}