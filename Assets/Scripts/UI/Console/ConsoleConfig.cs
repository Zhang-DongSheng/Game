using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ConsoleConfig
    {
        public const int LOGMAX = 200;

        public static Dictionary<string, string> Commands = new Dictionary<string, string>
        {
            {"help", "帮助"},
            {"get <color=red>propID</color> <color=blue>number</color>", "获取道具"},
            {"upgrade <color=red>level</color>", "升级"},
            {"pass", "通关"},
            {"vip up", "vip 升级"},
            {"vip down", "vip 降级"},
            {"kill <color=red>targetID</color>", "击杀指定目标"},
        };

        public static readonly Dictionary<string, string> Infomation = new Dictionary<string, string>
        {
            { "公司名称", Application.companyName },
            {"产品名称", Application.productName},
            {"游戏版本", Application.version},
            {"游戏包名", Application.identifier},

            {"设备模型", SystemInfo.deviceModel},
            {"设备类型", SystemInfo.deviceType.ToString()},
            {"设备名称", SystemInfo.deviceName},
            {"设备唯一标识符", SystemInfo.deviceUniqueIdentifier},

            {"操作系统", SystemInfo.operatingSystem},
            {"系统内存", SystemInfo.systemMemorySize.ToString()},
            {"网络类型", Application.internetReachability.ToString()},
            {"运行平台", Application.platform.ToString()},
            {"是否为移动平台", Application.isMobilePlatform.ToString()},
            {"是否支持后台运行", Application.runInBackground.ToString()},

            {"显卡ID", SystemInfo.graphicsDeviceID.ToString()},
            {"显卡类型", SystemInfo.graphicsDeviceType.ToString()},
            {"显卡名称", SystemInfo.graphicsDeviceName},
            {"显卡供应商", SystemInfo.graphicsDeviceVendor},
            {"显卡版本号", SystemInfo.graphicsDeviceVersion},
            {"显卡供应唯一ID", SystemInfo.graphicsDeviceVendorID.ToString()},
            {"显存内存大小", SystemInfo.graphicsMemorySize.ToString()},
            {"显卡是否支持多线程渲染", SystemInfo.graphicsMultiThreaded.ToString()},
            {"支持的渲染目标数量", SystemInfo.supportedRenderTargetCount.ToString()},
            {"支持最大图片尺寸", SystemInfo.maxTextureSize.ToString()},

            {"当前语言", Application.systemLanguage.ToString()},
            {"资源路径", Application.dataPath},
            {"文件存储路径", Application.persistentDataPath},
            { "UnityVersion", Application.unityVersion},
        };
    }
}