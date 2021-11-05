using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;

namespace Game.Test
{
    public class Test : MonoBehaviour
    {
        public List<Transform> list;

        public Vector3 position;

        private readonly List<Vector3> points = new List<Vector3>();

        private void Awake()
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
        [ContextMenu("Test")]
        public void OnClickContextMenu()
        {

        }

        private async Task StartAsync()
        {

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
        public float Weight { get { return weight; } }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }
    }
}