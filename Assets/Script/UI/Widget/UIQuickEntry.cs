using System;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static void OpenUIConfirm(string title, string message, Action confirm, Action cancel = null)
        {
            if (UIManager.Instance.GetCtrl(UIKey.UIConfirm) is UICtrl ctrl)
            {
                ctrl.Paramter(title, message, confirm, cancel);
            }
            UIManager.Instance.Open(UIKey.UIConfirm);
        }

        public static void OpenUINotice(string message, float time = -1)
        {
            if (UIManager.Instance.GetCtrl(UIKey.UINotice) is UICtrl ctrl)
            {
                ctrl.Paramter(message, time);
            }
            UIManager.Instance.Open(UIKey.UINotice);
        }
    }
}
