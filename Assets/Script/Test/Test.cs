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

        [Readonly] public Vector3 position;


        [Readonly(false)] public Quaternion rotation;

        private void Awake()
        {

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
            Dictionary<string, TestClass> dic = new System.Collections.Generic.Dictionary<string, TestClass>();

            List<TestClass> list = new List<TestClass>();

            Stack<TestClass> stack = new Stack<TestClass>();

            for (int i = 0; i < 10; i++)
            {
                TestClass item = new TestClass()
                {
                    name = string.Format("TC_{0}", i),
                };
                dic.Add(i.ToString(), item);

                list.Add(item);

                stack.Push(item);
            }

            var _strList = list.ToList<TestClass, string>((value) => { return value.name; });

            for (int i = 0; i < _strList.Count; i++)
            {
                Debug.LogError(_strList[i]);
            }
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