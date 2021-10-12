using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public List<Transform> list;

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

        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick()
        {
            float angle = MathUtils.PositionToAngle(list[0].position, list[1].position);

            Quaternion rotation = MathUtils.PositionToRotation(list[0].position, list[1].position);

            Debug.Log(string.Format("角度1：{0}，角度2：{1}", angle, rotation.eulerAngles));
        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("Test")]
        public void OnClickContextMenu()
        {
            this.Relevance();
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