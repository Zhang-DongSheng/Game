using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ILRuntime.Game.UI
{
    public class UITest : ILUIBase
    {
        private void Awake()
        {
            Debug.Log("启动");
        }

        private void Start()
        {
            Debug.Log("开始");
        }

        private void Update()
        {
            Debug.LogWarning("更新");
        }

        private void Destroy()
        {
            Debug.LogError("销毁");
        }
    }
}