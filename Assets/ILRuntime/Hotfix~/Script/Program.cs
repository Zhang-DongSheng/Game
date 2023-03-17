using ILRuntime.Game.UI;
using UnityEngine;

namespace ILRuntime.Game
{
    class Program
    {
        public static void Initialize()
        {
            Debug.LogError("我是测试代码！");

            GameObject go = new GameObject("Test");

            go.AddComponent<UITest>();
        }
    }
}
