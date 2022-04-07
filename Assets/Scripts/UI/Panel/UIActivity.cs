using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIActivity : UIBase
    {
        [SerializeField] private TabGroup tab;

        [SerializeField] private UIActivitySign viewSign;

        [SerializeField] private UIActivityCDKEY viewCDKEY;

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
