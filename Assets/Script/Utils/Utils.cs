using UnityEngine;

public class Utils
{
    /// <summary>
    /// 获取网络类型
    /// </summary>
    /// <returns>网络类型</returns>
    public static string Get_Network_Type()
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
