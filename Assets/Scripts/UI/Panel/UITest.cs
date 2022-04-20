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

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);

            btn_back.onClick.AddListener(OnCilckBack);

            toggle.onClick.AddListener(OnClickToggle);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UIQuickEntry.OpenUIConfirm("A", "B", () =>
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
                UIQuickEntry.OpenUIReward(new Data.Reward()
                {
                    title = "XXX",
                    currencies = new List<Currency>()
                    {
                        new Currency(CurrencyEnum.Gold,999),
                        new Currency(CurrencyEnum.Diamond,10),
                    },
                    props = new List<Prop>()
                    {
                        new Prop(){ parallelism = 0},
                        new Prop(){ parallelism = 1},
                        new Prop(){ parallelism = 2},
                        new Prop(){ parallelism = 3},
                        new Prop(){ parallelism = 4},
                    }
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
            UIManager.Instance.Open(UIPanel.UILogin);
        }

        private void OnClickToggle(int index)
        {

        }
    }
}