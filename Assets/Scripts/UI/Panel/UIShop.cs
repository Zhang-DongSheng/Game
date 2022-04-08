using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIShop : UIBase
    {
        [SerializeField] private TabGroup tab;

        private void Awake()
        {
            tab.Initialize(OnClickTab);
        }

        private void Start()
        {
            tab.Refresh(new int[5] { 1, 2, 3, 4, 5 });
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
