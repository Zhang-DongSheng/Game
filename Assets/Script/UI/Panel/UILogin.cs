using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILogin : UIBase
    {
        public Button btn_next;

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            UIManager.Instance.Close(UIKey.UILogin);

            UIManager.Instance.Open(UIKey.UIMain);
        }
    }
}