using System;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 通用
        /// </summary>
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
            /// 打开系统邮箱
            /// </summary>
            public static void OpenQQ(string qq)
            {
                if (string.IsNullOrEmpty(qq)) return;

                string url = string.Format("tencent://message/?uin={0}&Site=&Menu=yes", qq);

                Application.OpenURL(url);
            }
            /// <summary>
            /// 复制到剪切板
            /// </summary>
            public static void Copy(string content)
            {
                if (string.IsNullOrEmpty(content)) return;

                GUIUtility.systemCopyBuffer = content;
            }
            /// <summary>
            /// 内存
            /// </summary>
            public static int MemoryLevel()
            {
                int size = SystemInfo.systemMemorySize;

                if (size <= 1024 * 4)
                {
                    return 0;
                }
                else if (size >= 1024 * 8)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            /// <summary>
            /// 是否支持GPUInstance
            /// </summary>
            /// https://docs.unity3d.com/ScriptReference/SystemInfo-graphicsShaderLevel.html
            public static bool SupportGPUInstance()
            {
                return SystemInfo.graphicsShaderLevel >= 35;
            }
            /// <summary>
            /// 屏幕常亮
            /// </summary>
            public static void Sleep(bool sleep = false)
            {
                if (sleep)
                {
                    Screen.sleepTimeout = SleepTimeout.SystemSetting;
                }
                else
                {
                    Screen.sleepTimeout = SleepTimeout.NeverSleep;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public static void Quality(int level)
            {
                RenderSettings.fog = level > 2;

                QualitySettings.SetQualityLevel(level);
#if UNITY_IPHONE
                //抗锯齿过滤器可以设置为0, 2, 4, 8
                QualitySettings.antiAliasing = 0;
#endif
            }
        }
    }
}