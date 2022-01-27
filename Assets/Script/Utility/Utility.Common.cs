using System;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Common
        {
            /// <summary>
            /// 获取网络类型
            /// </summary>
            public static string GetNetworkType
            {
                get
                {
                    string net_type;

                    if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                    {
                        net_type = "WIFI";
                    }
                    else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                    {
                        net_type = "4G";
                    }
                    else
                    {
                        net_type = "UnKnow";
                    }

                    return net_type;
                }
            }
            /// <summary>
            /// 打开系统邮箱
            /// </summary>
            public static void OpenEmail(string email)
            {
                if (string.IsNullOrEmpty(email)) return;

                Uri uri = new Uri(string.Format("mailto:{0}?subject={1}", email, Application.productName));

                Application.OpenURL(uri.AbsoluteUri);
            }
            /// <summary>
            /// 复制到剪切板
            /// </summary>
            public static void Copy(string content)
            {
                if (string.IsNullOrEmpty(content)) return;

                GUIUtility.systemCopyBuffer = content;
            }
        }
    }
}