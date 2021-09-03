using System;
using UnityEngine;

namespace Game.Console
{
    public static class Config
    {
        public static readonly string Path_Log = string.Format("{0}/log_{1}.txt", Application.persistentDataPath, DateTime.Now.ToString("yyyyMMddhhmmss"));

        public static readonly string Path_Profiler = string.Format("{0}/profiler_{1}.log", Application.persistentDataPath, DateTime.Now.ToString("yyyyMMddhhmmss"));

        public static readonly string Path_ScreenCapture = string.Format("{0}/capture_{1}.png", Application.persistentDataPath, DateTime.Now.ToString("yyyyMMddhhmmss"));

        public static readonly string[] Title = new string[5] { "控制台", "日志", "游戏信息", "设置", "设备信息" };

        public static readonly string[] Command = new string[]
        {
            "Examples:",
            "1.获得物品 【Get <color=red>propID</color> <color=blue>number</color>】",
            "2.升级 【Level Up】",
            "3.过关 【Level Pass】",
            "4.升级VIP 【VIP Up】",
            "5.杀死指定目标 【Kill <color=red>targetID</color>】",
        };

        public static readonly string[] Infomation = new string[]
        {
            "<color=black>公司名称: </color>" + Application.companyName,
            "<color=black>产品名称: </color>" + Application.productName,
            "<color=black>游戏版本: </color>" + Application.version,
            "<color=black>游戏包名: </color>" + Application.identifier,

            "<color=black>设备模型: </color>" + SystemInfo.deviceModel,
            "<color=black>设备类型: </color>" + SystemInfo.deviceType,
            "<color=black>设备名称: </color>" + SystemInfo.deviceName,
            "<color=black>设备唯一标识符: </color>" + SystemInfo.deviceUniqueIdentifier,

            "<color=black>操作系统: </color>" + SystemInfo.operatingSystem,
            "<color=black>系统内存: </color>" + SystemInfo.systemMemorySize,
            "<color=black>网络类型: </color>" + Application.internetReachability,
            "<color=black>运行平台: </color>" + Application.platform,
            "<color=black>是否为移动平台: </color>" + Application.isMobilePlatform,
            "<color=black>是否支持后台运行: </color>" + Application.runInBackground,

            "<color=black>显卡ID: </color>" + SystemInfo.graphicsDeviceID,
            "<color=black>显卡类型: </color>" + SystemInfo.graphicsDeviceType,
            "<color=black>显卡名称: </color>" + SystemInfo.graphicsDeviceName,
            "<color=black>显卡供应商: </color>" + SystemInfo.graphicsDeviceVendor,
            "<color=black>显卡版本号: </color>" + SystemInfo.graphicsDeviceVersion,
            "<color=black>显卡供应唯一ID: </color>" + SystemInfo.graphicsDeviceVendorID,
            "<color=black>显存内存大小: </color>" + SystemInfo.graphicsMemorySize,
            "<color=black>显卡是否支持多线程渲染: </color>" + SystemInfo.graphicsMultiThreaded,
            "<color=black>支持的渲染目标数量: </color>" + SystemInfo.supportedRenderTargetCount,
            "<color=black>支持最大图片尺寸: </color>" + SystemInfo.maxTextureSize,

            "<color=black>当前语言: </color>" + Application.systemLanguage,
            "<color=black>资源路径: </color>" + Application.dataPath,
            "<color=black>文件存储路径: </color>" + Application.persistentDataPath,
            "<color=black>UnityVersion: </color>" + Application.unityVersion,
        };

        public const string Email = @"https://www.baidu.com";
    }

    [System.Serializable]
    public class LogData
    {
        public LogType type;

        public DateTime time;

        public string message;

        public string content;

        public LogData(LogType type, string message, string content)
        {
            this.type = type;
            this.time = DateTime.Now;
            this.message = message;
            this.content = content;
        }

        public string Message
        {
            get
            {
                switch (type)
                {
                    case LogType.Log:
                        return string.Format("<color=green>{0}</color>", message);
                    case LogType.Warning:
                        return string.Format("<color=yellow>{0}</color>", message);
                    case LogType.Error:
                        return string.Format("<color=red>{0}</color>", message);
                    case LogType.Exception:
                        return string.Format("<color=black>{0}</color>", message);
                    default:
                        return string.Format("<color=white>{0}</color>", message);
                }
            }
        }
    }
}