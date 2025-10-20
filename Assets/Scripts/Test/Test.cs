using Game.Attribute;
using Game.World;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Test
{
    [ExecuteInEditMode]
    public class Test : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        [SerializeField] private GameObject[] other;

        [SerializeField] private IntInterval intInterval;

        [SerializeField] private UIntPair xxx;

        [SerializeField] private IntPair xxxxx;

        [SerializeField] private Pair<int, float> xxfdf;

        [SerializeField] private Vector3 position;
        [Line("#00FFFF", 2)]
        [Interval(1,10)]
        public Vector2Int vector1;
        [FieldName("半径", true)]
        public float radius;
        [Interval(0, 360)]
        public float angle;
        [FieldName("杀马特",true)]
        public int vector4;
        [Suffix("(m/s)")]
        public int vector5;
        [Button("OnClickButton")]
        public float index;

        private Vector2 delta;

        private void Start()
        {
            var _ = StartAsync();
        }

        private void OnValidate()
        {
            xxx = xxxxx;
        }

        private void OnDrawGizmos()
        {
            if (target != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(target.transform.position, radius);
                var position = target.transform.position;
                var points = EntityUtils.PickPoints(position, radius);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(points[0], 0.5f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(points[1], 0.5f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(points[2], 0.5f);
            }
        }

        private void Update()
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
            var vector = new Vector3(1, 0, 1);

            var offset = new Vector2(1, 2f);

            if (Mathf.Abs(offset.x) > 0.01f)
                vector = Quaternion.AngleAxis(offset.x, Vector3.up) * vector;
            if (Mathf.Abs(offset.y) > 0.01f)
                vector = Quaternion.AngleAxis(offset.y, Vector3.left) * vector;

            Debuger.LogError(Author.Test, vector);
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
            var position = target.transform.position;

            var rotation = EntityUtils.Orientation(position, radius);

            target.transform.position = position;

            target.transform.rotation = rotation;
        }

        private async Task StartAsync()
        {
            await Task.Run(() =>
            {
                
            });
        }
    }
}