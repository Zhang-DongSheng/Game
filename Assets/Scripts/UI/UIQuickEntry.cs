using Data;
using System;
using System.Diagnostics;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static bool async = GameConfig.Load == Resource.LoadType.AssetBundle;

        public static void Open(UIPanel panel, Paramter paramter = null)
        {
            UIManager.Instance.Paramter(panel, paramter);
            {
                UIManager.Instance.Open(panel, async: async);
            }
        }

        public static void OpenSingle(UIPanel panel, Paramter paramter = null)
        {
            UIManager.Instance.CloseAll();

            UIManager.Instance.Paramter(panel, paramter);
            {
                UIManager.Instance.Open(panel, async: async);
            }
        }

        public static void OpenUIConfirm(string title, string message, Action confirm, Action cancel = null)
        {
            Open(UIPanel.UIConfirm, new Paramter()
            {
                ["title"] = title,
                ["message"] = message,
                ["confirm"] = confirm,
                ["cancel"] = cancel,
            });
        }

        public static void OpenUINotice(string notice)
        {
            Open(UIPanel.UINotice, new Paramter()
            {
                ["notice"] = notice,
            });
        }

        public static void OpenUITips(string tips)
        {
            Open(UIPanel.UITips, new Paramter()
            {
                ["tips"] = tips,
            });
        }

        public static void OpenUIHorseLamp(string message, float time = -1)
        {
            Open(UIPanel.UIHorseLamp, new Paramter()
            {
                ["message"] = message,
            });
        }

        public static void OpenUIReward(Reward reward)
        {
            Open(UIPanel.UIReward, new Paramter()
            {
                ["reward"] = reward,
            });
        }
    }
}