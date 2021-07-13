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
        [FormerlySerializedAs("position")]
        public Vector2 position;

        [Interval(1, 10)]
        public float tt;

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
            
        }
    }
}