using Game;
using Job;
using System;
using System.Collections.Generic;
using Unity.Jobs;
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
            NumberJob job = new NumberJob(NumberJob.Command.Sum, 1, 2, 3, 4, 5);

            JobHandle handle = job.Schedule();

            handle.Complete();

            Debug.Log(job.result[0]);
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

    public class TestClass : IWeight
    {
        public string name;

        public float weight;

        public string XX
        {
            get; set;
        }
        public float Weight { get { return weight; } }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }
    }
}