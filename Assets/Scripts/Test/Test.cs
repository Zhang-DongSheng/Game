using Game.Attribute;
using Game.Network;
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
            int index = Utility.Enum.Index<TT>(2);

            Debuger.LogError(Author.Test, index);

            TT t = Utility.Enum.FromString<TT>("Two");

            Debuger.LogError(Author.Test, t.ToString());
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
        public float value { get { return weight; } }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
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