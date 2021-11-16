using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
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

        [SerializeField] private List<Transform> list;

        private readonly List<Vector3> points = new List<Vector3>();

        public RawImage image;

        private void Awake()
        {
            image.texture = Device.WebCamTextureDevice.Instance.Camera;

            Device.WebCamTextureDevice.Instance.Begin();

            //Device.MicrophoneDevice.Instance.Init();
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
            Transform child = list[0].FindByName("Item (7)", false);

            if (child != null)
            {
                Debuger.LogError(Author.Test, child.name);
            }
            else
            {
                Debuger.LogError(Author.Test, "没找到");
            }
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