using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        [Interval(1, 10)]
        public Vector2 position;
        [Line(AttributeColor.Green)]
        [Interval(1, 10)]
        public float tt;

        public GameObject test;

        [Curve(AttributeColor.Gray)]
        public AnimationCurve curve;

        public ImageFade fade;

        public List<int> sprites;

        private string value;

        public RenewableAsset asset;

        private readonly List<string> list = new List<string>()
        {
            "ceo",
            "cts",
            "deep",
            "deeplove",
            "dest",
            "dream",
            "hust",
            "las",
            "pd"
        };

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
            int index = UnityEngine.Random.Range(0, list.Count);

            string key = list[index];

            asset.CreateAsset(string.Format("android/picture/dynamic/dynamic_{0}", key), key);
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