using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITest : UIBase
    {
        public RawImage image;

        public ToggleGroupHelper toggle;

        public Button btn_next;

        public Button btn_back;

        private int index;

        protected override void OnAwake()
        {
            btn_next.onClick.AddListener(OnClick);

            btn_back.onClick.AddListener(OnCilckBack);

            toggle.onClick.AddListener(OnClickToggle);
        }

        private void Start()
        {
            
        }

        protected override void OnUpdate(float delta)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                index++;

                UIQuickEntry.OpenUIHorseLamp(string.Format("当前测试第{0}条消息！", index));
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                UIQuickEntry.Open(UIPanel.UILotteryDraw);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                UIQuickEntry.OpenUINotice("test");
            }
        }

        private void OnCilckBack()
        {
            UIManager.Instance.Back();
        }

        private void OnClick()
        {
            UIQuickEntry.Open(UIPanel.UILogin);
        }

        private void OnClickToggle(int index)
        {

        }
    }
}