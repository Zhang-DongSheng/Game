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
            //Debuger.Log("启动");
        }

        private void Start()
        {
            //Debuger.Log("开始");
        }

        private void Update()
        {
            //Debuger.LogWarning("更新");
        }

        private void Destroy()
        {
            //Debuger.LogError("销毁");
        }
    }
}