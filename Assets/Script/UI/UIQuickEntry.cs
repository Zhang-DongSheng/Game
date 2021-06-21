using System;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static void OpenUIConfirm(string title, string message, Action confirm, Action cancel = null)
        {
            if (UIManager.Instance.GetCtrl(UIPanel.UIConfirm) is UICtrlBase ctrl)
            {
                ctrl.Paramter(title, message, confirm, cancel);
            }
            UIManager.Instance.Open(UIPanel.UIConfirm);
        }

        public static void OpenUINotice(string message, float time = -1)
        {
            if (UIManager.Instance.GetCtrl(UIPanel.UINotice) is UICtrlBase ctrl)
            {
                ctrl.Paramter(message, time);
            }
            UIManager.Instance.Open(UIPanel.UINotice);
        }
    }
}
