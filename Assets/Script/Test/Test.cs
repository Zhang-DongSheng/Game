using Game;
using Game.Network;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public RectTransform rect;

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
            rect.Full();
        }

        private void Update()
        {
            
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            P p = new P();

            p.SetField("a", "我是a");

            p.SetField("b", "xx");

            p.SetField("c", 2);

            p.Call("D");

            p.Call("E", 1);
        }
    }

    public class P
    {
        private string a;

        private int b;

        private float c;

        private void D()
        {
            Debug.LogError(a);
        }

        private void E( int value)
        {
            Debug.LogError(b + value);
        }
    }
}