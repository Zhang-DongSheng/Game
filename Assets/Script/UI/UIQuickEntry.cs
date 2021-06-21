using System;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static void OpenUIConfirm(string title, string message, Action confirm, Action cancel = null)
        {
            if (UIManager.Instance.GetCtrl(UIPanel.UIConfirm) is CtrlBase ctrl)
            {
                Paramter paramter = new Paramter()
                {
                    ["title"] = title,
                    ["message"] = message,
                    ["confirm"] = confirm,
                    ["cancel"] = cancel,
                };
                ctrl.Paramter(paramter);
            }
            UIManager.Instance.Open(UIPanel.UIConfirm);
        }

        public static void OpenUINotice(string message, float time = -1)
        {
            if (UIManager.Instance.GetCtrl(UIPanel.UINotice) is CtrlBase ctrl)
            {
                Paramter paramter = new Paramter()
                {
                    ["message"] = message,
                };
                ctrl.Paramter(paramter);
            }
            UIManager.Instance.Open(UIPanel.UINotice);
        }

        public static void OpenUIReward(string title)
        {
            if (UIManager.Instance.GetCtrl(UIPanel.UIReward) is CtrlBase ctrl)
            {
                Paramter paramter = new Paramter()
                {
                    ["title"] = title,
                };
                ctrl.Paramter(paramter);
            }
            UIManager.Instance.Open(UIPanel.UIReward);
        }
    }
}