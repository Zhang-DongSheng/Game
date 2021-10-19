using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Game.Test
{
    public class Test : MonoBehaviour
    {
        public List<Transform> list;

        public Vector3 position;

        private void Awake()
        {
            UnregularScrollList scroll = GetComponent<UnregularScrollList>();

            List<string> list = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                list.Add(i.ToString());
            }
            scroll.Refresh(list);
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClick();
            }
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            
        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick()
        {
            
        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("Test")]
        public void OnClickContextMenu()
        {
            
        }
    }

    public class TestClass
    {
        public string name;

        public string XX
        {
            get; set;
        }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }
    }
}