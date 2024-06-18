using Game.Resource;
using System;
using UnityEngine;

namespace Game.UI
{
    public static class UIQuickEntry
    {
        public static bool async = ResourceConfig.Loading == LoadingType.AssetBundle;

        public static void Open(UIPanel panel, UIParameter parameter = null, Action callback = null)
        {
            UIManager.Instance.Parameter((int)panel, parameter);
            {
                UIManager.Instance.Open((int)panel, async, callback);
            }
        }

        public static void OpenSingle(UIPanel panel, UIParameter parameter = null, Action callback = null)
        {
            UIManager.Instance.CloseAll();

            UIManager.Instance.Parameter((int)panel, parameter);
            {
                UIManager.Instance.Open((int)panel, async, callback);
            }
        }

        public static void OpenConfirmView(string title, string message, Action confirm, Action cancel = null)
        {
            Open(UIPanel.Confirm, new UIParameter()
            {
                ["title"] = title,
                ["message"] = message,
                ["confirm"] = confirm,
                ["cancel"] = cancel,
            });
        }

        public static void OpenNoticeView(string notice)
        {
            Open(UIPanel.Notice, new UIParameter()
            {
                ["notice"] = notice,
            });
        }

        public static void OpenHorseLampView(string message, float time = -1)
        {
            Open(UIPanel.HorseLamp, new UIParameter()
            {
                ["message"] = message,
            });
        }

        public static void OpenRewardView(Reward reward)
        {
            Open(UIPanel.Reward, new UIParameter()
            {
                ["reward"] = reward,
            });
        }

        public static void OpenBubbleView(Transform transform, string message)
        {
            Open(UIPanel.Bubble, new UIParameter()
            {
                ["transform"] = transform,
                ["value"] = message,
            });
        }

        public static bool IsOpen(UIPanel panel)
        {
            if (UIManager.Instance.TryGetCtrl((int)panel, out var ctrl))
            {
                return ctrl.active;
            }
            return false;
        }
    }
}