using Data;
using Game.Resource;
using System;
using UnityEngine;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static bool async = ResourceConfig.Loading == LoadingType.AssetBundle;

        public static void Open(UIPanel panel, UIParameter parameter = null)
        {
            UIManager.Instance.Parameter(panel, parameter);
            {
                UIManager.Instance.Open(panel, async);
            }
        }

        public static void OpenSingle(UIPanel panel, UIParameter parameter = null)
        {
            UIManager.Instance.CloseAll();

            UIManager.Instance.Parameter(panel, parameter);
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

        public static void OpenUIBubble(Transform transform, string message)
        {
            Open(UIPanel.UIBubble, new UIParameter()
            {
                ["transform"] = transform,
                ["value"] = message,
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