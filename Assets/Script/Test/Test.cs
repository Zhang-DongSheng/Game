using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;

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

        [SerializeField] private RayTouch touch;

        [SerializeField] private Command command;

        [SerializeField] private List<Transform> list;

        [SerializeField] private Vector3 position;

        [SerializeField, Range(0, 30f)] private float speed = 1;

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

            switch (command)
            {
                case Command.One:
                    position = TransformUtils.MoveTowards(position, touch.Destination / 30f, Time.deltaTime * speed);
                    break;
                case Command.Two:
                    position = TransformUtils.MoveTowards1(position, touch.Destination / 30f, Time.deltaTime * speed);
                    break;
                case Command.Three:
                    position = TransformUtils.MoveTowards2(position, touch.Destination / 30f, Time.deltaTime * speed);
                    break;
                case Command.Four:
                    position = TransformUtils.MoveTowards3(position, touch.Destination / 30f, speed);
                    break;
                case Command.Five:

                    break;
                default:

                    break;
            }
            list[0].position = position;
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