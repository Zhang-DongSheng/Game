using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public Transform test;

        public Route route;

        private readonly ParaCurves curves = new ParaCurves();

        private void Awake()
        {
            route.onComplete.AddListener(() =>
            {
                Debug.LogError("完成！");
            });
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
            Vector3 position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-5, 5));

            List<Vector3> routes = curves.FetchCurves(Vector3.zero, position, 36, 10);

            route.Startup(test, routes, 36);
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

    public class T2
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