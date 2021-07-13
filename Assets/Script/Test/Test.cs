using Game;
using Game.Network;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

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
                int index = Random.Range(0, sprites.Count);

                //fade.Fade(sprites[index]);
            }
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            T2 t2 = new T2();

            t2.SetMember("name", "test");

            t2.SetMember("XX", "ddddd");

            t2.Call("Do");


            string x1 = t2.GetMember("name") as string;

            string x2 = t2.GetMember("XX") as string;

            Debug.LogError(x1 + "[ 2 ]" + x2);
        }
    }

    public class T2
    {
        public string name;

        public string XX
        {
            get;set;
        }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }
    }
}