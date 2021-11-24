using System.Collections.Generic;
using System.Threading.Tasks;
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
            
        }

        private void OnEnable()
        {
            Data.DataBone bone = UnityEditor.AssetDatabase.LoadAssetAtPath<Data.DataBone>("Assets/Package/Data/Bone.asset");


            Debuger.LogError(Author.Test, bone.dic.Count);
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
            Vector3 position = new Vector3(1, 2, 3);

            Quaternion rotation = Quaternion.Euler(30, 0, 0);

            Vector3 scale = new Vector3(1, 1, 1);

            var spaceConvertMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

            var origMatrix = Matrix4x4.TRS(position, rotation, scale);

            var matrix = spaceConvertMatrix * origMatrix;

            position = matrix.GetRow(3);   new Vector3(matrix[0, 3], matrix[1, 3], matrix[2, 3]);

            rotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));

            Debuger.Log(Author.Test, matrix);

            //Debuger.Log(Author.Test, rotation);

            //Debuger.Log(Author.Test, scale);
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