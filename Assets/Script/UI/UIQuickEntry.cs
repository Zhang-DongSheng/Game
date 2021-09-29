using Data;
using System;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static void OpenUIConfirm(string title, string message, Action confirm, Action cancel = null)
        {
            UIManager.Instance.Paramter(UIPanel.UIConfirm, new Paramter()
            {
                ["title"] = title,
                ["message"] = message,
                ["confirm"] = confirm,
                ["cancel"] = cancel,
            });
            UIManager.Instance.Open(UIPanel.UIConfirm);
        }

        public static void OpenUINotice(string notice)
        {
            UIManager.Instance.Paramter(UIPanel.UINotice, new Paramter()
            {
                ["notice"] = notice,
            });
            UIManager.Instance.Open(UIPanel.UINotice);
        }

        public static void OpenUITips(string tips)
        {
            UIManager.Instance.Paramter(UIPanel.UITips, new Paramter()
            {
                ["tips"] = tips,
            });
            UIManager.Instance.Open(UIPanel.UITips);
        }

        public static void OpenUIHorseLamp(string message, float time = -1)
        {
            UIManager.Instance.Paramter(UIPanel.UIHorseLamp, new Paramter()
            {
                ["message"] = message,
            });
            UIManager.Instance.Open(UIPanel.UIHorseLamp);
        }

        public static void OpenUIReward(Reward reward)
        {
            UIManager.Instance.Paramter(UIPanel.UIReward, new Paramter()
            {
                ["reward"] = reward,
            });
            UIManager.Instance.Open(UIPanel.UIReward);
        }
    }
}