using Data.Serializable;
using Game.Attribute;
using Game.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Test
{
    public class Test : ItemBase
    {
        [SerializeField] private Vector3 position;
        [Button("OnClickButton")]
        public float index;

        private Vector2 delta;

        private void OnDrawGizmos()
        {
            
        }

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
            
        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick(int code)
        {
            Debuger.Log(Author.Test, "点击测试" + code);

            bool active = code == 1;

            gameObject.SetActive(active, Extension.VisibleType.Cull);
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
            string path = "Package/Prefab/Model/Bullet/Bullet.prefab";

            for (int i = 0; i < (int)index; i++)
            {
                PoolManager.Instance.Pop(path);
            }
        }

        private async System.Threading.Tasks.Task StartAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Debuger.Log(Author.Test, "I'm fine!");
            });
        }
    }
}