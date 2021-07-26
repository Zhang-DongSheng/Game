using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

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

        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("Test")]
        public void OnClickContextMenu()
        {
            Vector4 space = RectTransformUtils.Space(target);

            Vector2 position = Vector2.zero;

            position.x = space.x + space.y;

            position.y = space.z + space.w;

            position *= -0.5f;

            target.anchoredPosition = position;

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, space.y - space.x);

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, space.w - space.z);
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