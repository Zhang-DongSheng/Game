using Game.Attribute;
using Game.Network;
using Game.Pool;
using Protobuf;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.Test
{
    public class Test : RuntimeBase
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

        [SerializeField] private List<Transform> items;

        [Button("OnClickButton")]
        public float index;

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
            Debuger.Log(Author.Test, "___________________________________");
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            for (int i = 0; i < 5; i++)
            {
                Debuger.Log(Author.Test, Utility._Random.Range(1, 5));
            }

            Debuger.Log(Author.Test, "___________________________________");
        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick()
        {
            Person person = new Person()
            {
                Name = "张三",
                Age = 18,
            };

            string content = Network.Convert.Serialize(person);


            NetworkManager.Instance.Send(content);
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
}