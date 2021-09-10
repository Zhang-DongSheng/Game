using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ILRuntime.View
{
    public class TestView : ILRuntimeMonoBehaviour
    {
        public void Start()
        {
            Debug.Log("开始");
        }

        public void Update()
        {
            Debug.LogWarning("更新");
        }
    }
}