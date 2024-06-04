using Game.Attribute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Test
{
    public class Test : ItemBase
    {
        [SerializeField] private GameObject target;

        [SerializeField] private IntInterval intInterval;

        [SerializeField] private Vector3 position;
        [Line("#00FFFF", 2)]
        [Interval(1,10)]
        public Vector2Int vector1;
        [Interval(1, 10)]
        public Vector2 vector2;
        [Interval(1, 10)]
        public float vector3;
        [Display("杀马特",true)]
        public int vector4;
        [Suffix("(m/s)")]
        public int vector5;

        public int vector6;
        [Button("OnClickButton")]
        public float index;

        

        private Vector2 delta;

        private void Start()
        {
            var _ = StartAsync();
        }

        protected override void OnUpdate(float delta)
        {
            this.delta.x = Input.GetAxis("Horizontal");

            this.delta.y = Input.GetAxis("Vertical");
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnClick(0);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClick(1);
            }
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            int value = 599;

            if (paramters.Length > 0)
            { 
                int.TryParse(paramters[0], out value);
            }

            List<int> list = new List<int>();

            for (int i = 0; i < 100000; i++)
            {
                list.Add(i);
            }

            int compare(int x)
            {
                if (x == value)
                {
                    return 0;
                }
                else
                {
                    return x > value ? 1 : -1;
                }
            }

            var watch = Stopwatch.StartNew();

            watch.Start();

            var result = Game.Utility.Search.SequenceSearch(list, x => x == value);

            Debuger.LogError(Author.Test, "1 结果" + result);

            watch.Stop();

            Debuger.LogError(Author.Test, "1 花费时间" + watch.ElapsedTicks);

            watch.Restart();

            result = Game.Utility.Search.BinarySearch(list, compare);

            Debuger.LogError(Author.Test, "2 结果" + result);

            watch.Stop();

            Debuger.LogError(Author.Test, "2 花费时间" + watch.ElapsedTicks);

        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick(int code)
        {
            
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
            
        }

        private async System.Threading.Tasks.Task StartAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                
            });
        }
    }
}