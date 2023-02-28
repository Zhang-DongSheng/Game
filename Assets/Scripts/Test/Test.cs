using Data;
using Game.Attribute;
using Game.Network;
using Game.Pool;
using Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Test
{
    public class Test : RuntimeBehaviour
    {
        [SerializeField] private Vector3 position;

        [Button("OnClickButton")] public float index;

        protected void Awake()
        {
            NetworkEventManager.Register(NetworkEventKey.Test, OnReceiveMessage);

            NetworkManager.Instance.Connection();
        }

        private void OnDrawGizmos()
        {
            
        }

        private void Start()
        {
            var _ = StartAsync();
        }

        protected override void OnUpdate(float delta)
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
            float x = float.Parse(paramters[0]);

            float v = Mathf.Pow(x, 0.2f) * 1000 - 900;

            Debuger.Log(Author.Test, v);
        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick()
        {
            string key = "Package/Prefab/Model/Role/MODEL ANIMATION.prefab";

            var go = PoolManager.Instance.Pop(key);

            
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

        private void OnReceiveMessage(NetworkEventHandle handle)
        {
            string content = Network.Convert.ToString(handle.buffer);

            Debuger.Log(Author.Test, content);

            Person person = Network.Convert.Deserialize<Person>(content);

            Debuger.Log(Author.Test, person.Name);
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

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }

        public float Value()
        {
            return weight;
        }
    }

    public enum TT
    { 
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
    }
}