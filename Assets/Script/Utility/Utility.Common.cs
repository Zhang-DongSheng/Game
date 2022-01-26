using System;
using System.IO;
using System.Text;
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
            /// <param name="email">邮箱</param>
            public static void OpenEmail(string email)
            {
                if (string.IsNullOrEmpty(email)) return;

                Uri uri = new Uri(string.Format("mailto:{0}?subject={1}", email, Application.productName));

                Application.OpenURL(uri.AbsoluteUri);
            }
            /// <summary>
            /// 复制到剪切板
            /// </summary>
            /// <param name="content">文本</param>
            public static void Copy(string content)
            {
                if (string.IsNullOrEmpty(content)) return;

                GUIUtility.systemCopyBuffer = content;
            }

            public static Encoding Encoding(string path)
            {
                var buffer = new byte[4];

                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    file.Read(buffer, 0, 4);
                }
                if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76) return System.Text.Encoding.UTF7;
                if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf) return System.Text.Encoding.UTF8;
                if (buffer[0] == 0xff && buffer[1] == 0xfe) return System.Text.Encoding.Unicode;
                if (buffer[0] == 0xfe && buffer[1] == 0xff) return System.Text.Encoding.BigEndianUnicode;
                if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff) return System.Text.Encoding.UTF32;

                return System.Text.Encoding.ASCII;
            }
        }
    }
}