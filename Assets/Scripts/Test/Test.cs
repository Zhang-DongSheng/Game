using Game.Attribute;
using System.Threading.Tasks;
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
            for (int i = 0; i < 10; i++)
            {
                int index = i;

                int value = Random.Range(1, 30);

                TimeManager.Instance.Register("T" + index, (long)(Time.time + value), () =>
                {
                    Debug.LogError("当前是" + value + "/" + index);
                });
            }
            Debug.LogError("开始");
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

        private async Task StartAsync()
        {
            await Task.Run(() =>
            {
                
            });
        }
    }
}