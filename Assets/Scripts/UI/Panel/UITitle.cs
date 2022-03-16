using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private Button btnBack;

        [SerializeField] private Button btnShop;

        private void Awake()
        {
            btnBack.onClick.AddListener(OnClickBack);

            btnShop.onClick.AddListener(OnClickShop);
        }

        private void Refresh()
        { 
        
        }

        private void OnClickBack()
        {
            UIManager.Instance.Back();
        }

        private void OnClickShop()
        {
            UIManager.Instance.Open(UIPanel.UIShop);
        }
    }
}