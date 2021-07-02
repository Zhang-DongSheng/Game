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
            string test = "Assets/like/test.meta";

            string a = FileUtils.UnityToSystem(test);

            string b = FileUtils.SystemToUnity(a);

            Debug.LogError(a);

            Debug.LogError(b);
        }
    }
}