using Data;
using Game.Resource;
using System;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static bool async = ResourceConfig.Loading == LoadingType.AssetBundle;

        public static void Open(UIPanel panel, UIParameter paramter = null)
        {
            UIManager.Instance.Paramter(panel, paramter);
            {
                UIManager.Instance.Open(panel, async);
            }
        }

        public static void OpenSingle(UIPanel panel, UIParameter paramter = null)
        {
            UIManager.Instance.CloseAll();

            UIManager.Instance.Paramter(panel, paramter);
            {
                UIManager.Instance.Open(panel, async);
            }
        }

        public static void OpenUIConfirm(string title, string message, Action confirm, Action cancel = null)
        {
            Open(UIPanel.UIConfirm, new UIParameter()
            {
                ["title"] = title,
                ["message"] = message,
                ["confirm"] = confirm,
                ["cancel"] = cancel,
            });
        }

        public static void OpenUINotice(string notice)
        {
            Open(UIPanel.UINotice, new UIParameter()
            {
                ["notice"] = notice,
            });
        }

        public static void OpenUITips(string tips)
        {
            Open(UIPanel.UITips, new UIParameter()
            {
                ["tips"] = tips,
            });
        }

        public static void OpenUIHorseLamp(string message, float time = -1)
        {
            Open(UIPanel.UIHorseLamp, new UIParameter()
            {
                ["message"] = message,
            });
        }

        public static void OpenUIReward(Reward reward)
        {
            Open(UIPanel.UIReward, new UIParameter()
            {
                ["reward"] = reward,
            });
        }

        public static bool IsOpen(UIPanel panel)
        {
            if (UIManager.Instance.TryGetCtrl(panel, out var ctrl))
            {
                return ctrl.active;
            }
            return false;
        }
    }
}