using Game.Attribute;
using Game.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.Test
{
    public class Test : MonoBehaviour
    {
        enum Command
        {
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
        }

        
        [SerializeField] private Command command;

        [SerializeField] private Vector3 position;

        [SerializeField, Range(0, 30f)] private float speed = 1;

        [SerializeField] private List<Transform> items;

        [Button("OnClickButton")]
        public float index;

        protected void Awake()
        {
            
        }

        private void OnEnable()
        {
           
        }

        private void OnDisable()
        {
            
        }

        private void OnDrawGizmos()
        {
            
        }

        private void Start()
        {
            var _ = StartAsync();
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
            RectTransform cell = items[0].GetComponent<RectTransform>();

            switch (command)
            {
                case Command.One:

                    Vector2 delta = new Vector2()
                    {
                        x = 20,
                        y = 0
                    };
                    cell.sizeDelta = delta * -1f;

                    position = new Vector2()
                    {
                        x = 20,
                        y = 0,
                    };
                    cell.anchoredPosition = position * 0.5f;

                    break;
                case Command.Two:

                    cell.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, cell.rect.width + 20);

                    break;
                case Command.Three:

                    cell.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 20, cell.rect.width - 20);
                    //3043273231
                    break;
            }



        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("OnClick")]
        public void OnClickContextMenu()
        {

        }

        public void OnClickButton(float index)
        {
            Debug.LogError("成功调用" + index);
        }

        private async System.Threading.Tasks.Task StartAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Debuger.Log(Author.Test, "I'm fine!");
            });
        }

        private void OnUpdate(float delta)
        {
            Debuger.Log(Author.Test, delta);
        }

        private void OnUpdate2(float delta)
        {
            Debuger.LogError(Author.Test, delta);
        }
    }

    [System.Serializable]
    public class TestClass : IWeight
    {
        public string name;

        public float weight;

        public string XX
        {
            get; set;
        }
        public float value { get { return weight; } }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }
    }
}