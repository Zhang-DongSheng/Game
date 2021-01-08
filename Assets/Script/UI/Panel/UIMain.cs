using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        public Button btn_next;

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            UIManager.Instance.Open<UITest>("");
        }
    }
}
