using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Factory;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITest : UIBase
    {
        public Button btn_next;

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            UIManager.Instance.Close(UIKey.UITest);

            UIManager.Instance.Open(UIKey.UILogin);
        }
    }
}