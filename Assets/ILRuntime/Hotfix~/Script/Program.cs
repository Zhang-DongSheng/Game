using ILRuntime.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ILRuntime
{
    class Program
    {
        public static void Initialize()
        {
            Debug.LogError("我是测试代码！");

            GameObject go = new GameObject("Test");

            go.AddComponent<TestView>();
        }
    }
}
