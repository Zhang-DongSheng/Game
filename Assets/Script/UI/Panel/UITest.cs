using System.Collections;
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

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);

            btn_back.onClick.AddListener(OnCilckBack);

            toggle.onClick.AddListener(OnClickToggle);
        }

        private void Start()
        {
            GameController.Instance.model.Show();

            image.texture = GameController.Instance.model.Texture;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UIQuickEntry.OpenUIConfirm("A","B",()=>
                {
                    Debug.LogError(Random.Range(0, 100));
                });
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                index++;

                UIQuickEntry.OpenUIHorseLamp(string.Format("当前测试第{0}条消息！", index));
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                UIQuickEntry.OpenUIReward(new Data.RewardInformation()
                {
                    title = "XXX",
                });
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                UIManager.Instance.Open(UIPanel.UILotteryDraw);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                UIQuickEntry.OpenUITips("test");
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
            //UIManager.Instance.Close(UIKey.UITest);

            GameController.Instance.model.Hide();

            UIManager.Instance.Open(UIPanel.UILogin);
        }

        private void OnClickToggle(int index)
        {
            
        }
    }
}