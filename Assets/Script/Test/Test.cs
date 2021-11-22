using Study;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Test
{
    public class Test : UI.UIBase
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

        [SerializeField] private List<Transform> list;

        public ButtonPro buttonPro;

        private readonly List<Vector3> points = new List<Vector3>();

        private void Awake()
        {
            buttonPro.onEnter.AddListener(() =>
            {
                Debuger.Log(Author.Test, "按钮按下");
            });

            buttonPro.onLeave.AddListener(() =>
            {
                Debuger.Log(Author.Test, "按钮抬起");
            });
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void OnDrawGizmos()
        {
            int count = points.Count;

            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    Gizmos.DrawLine(points[i], points[0]);
                }
                else
                {
                    Gizmos.DrawLine(points[i], points[i + 1]);
                }
                Gizmos.DrawSphere(points[i], 1f);
            }
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

        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("OnClick")]
        public void OnClickContextMenu()
        {

        }

        private async Task StartAsync()
        {
            await Task.Run(() =>
            {
                Debuger.Log(Author.Test, "I'm fine!");
            });
        }
    }

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