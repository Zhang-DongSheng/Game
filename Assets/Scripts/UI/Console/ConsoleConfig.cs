using UnityEngine;

namespace Game
{
    public class ConsoleConfig
    {
        public static readonly string[] Command = new string[]
        {
            "help:",
            "get <color=red>propID</color> <color=blue>number</color>",
            "upgrade <color=red>level</color>",
            "pass ",
            "vip up",
            "vip down",
            "kill <color=red>targetID</color>",
        };

        public static readonly string[] Infomation = new string[]
        {
            "公司名称" + Application.companyName,
            "产品名称" + Application.productName,
            "游戏版本" + Application.version,
            "游戏包名" + Application.identifier,

            "设备模型" + SystemInfo.deviceModel,
            "设备类型" + SystemInfo.deviceType,
            "设备名称" + SystemInfo.deviceName,
            "设备唯一标识符" + SystemInfo.deviceUniqueIdentifier,

            "操作系统" + SystemInfo.operatingSystem,
            "系统内存" + SystemInfo.systemMemorySize,
            "网络类型" + Application.internetReachability,
            "运行平台" + Application.platform,
            "是否为移动平台" + Application.isMobilePlatform,
            "是否支持后台运行" + Application.runInBackground,

            "显卡ID" + SystemInfo.graphicsDeviceID,
            "显卡类型" + SystemInfo.graphicsDeviceType,
            "显卡名称" + SystemInfo.graphicsDeviceName,
            "显卡供应商" + SystemInfo.graphicsDeviceVendor,
            "显卡版本号" + SystemInfo.graphicsDeviceVersion,
            "显卡供应唯一ID" + SystemInfo.graphicsDeviceVendorID,
            "显存内存大小" + SystemInfo.graphicsMemorySize,
            "显卡是否支持多线程渲染" + SystemInfo.graphicsMultiThreaded,
            "支持的渲染目标数量" + SystemInfo.supportedRenderTargetCount,
            "支持最大图片尺寸" + SystemInfo.maxTextureSize,

            "当前语言" + Application.systemLanguage,
            "资源路径" + Application.dataPath,
            "文件存储路径" + Application.persistentDataPath,
            "UnityVersion" + Application.unityVersion,
        };
    }
}