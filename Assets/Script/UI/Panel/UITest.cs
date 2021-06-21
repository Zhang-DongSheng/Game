using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Factory;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITest : UIBase
    {
        public RawImage image;

        public Button btn_next;

        public ToggleGroupHelper toggle;

        private int index;

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);

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

                UIQuickEntry.OpenUINotice(string.Format("当前测试第{0}条消息！", index));
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
        }

        private void OnClick()
        {
            //UIManager.Instance.Close(UIKey.UITest);

            GameController.Instance.model.Hide();

            UIManager.Instance.Open(UIPanel.UILogin);
        }

        private void OnClickToggle(int index)
        {
            Debug.LogError(index);
        }
    }
}