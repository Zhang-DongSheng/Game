using Game.State;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemEntry : ItemBase
    {
        [SerializeField] private UIPanel panel;

        [SerializeField] private Button button;

        [SerializeField] private GameObject mask;

        [SerializeField] private ItemReddot reddot;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public virtual void Refresh()
        {
            reddot.Refresh();

            SetActive(mask, false);
        }

        protected virtual void OnClick()
        {
            switch (panel)
            {
                case UIPanel.None:
                    {
                        UIQuickEntry.OpenUINotice("新功能开发中！敬请期待");
                    }
                    break;
                case UIPanel.UIDeploy:
                    {
                        GameStateController.Instance.EnterState<GameDeployState>();
                    }
                    break;
                default:
                    {
                        UIQuickEntry.Open(panel);
                    }
                    break;
            }
        }
    }
}