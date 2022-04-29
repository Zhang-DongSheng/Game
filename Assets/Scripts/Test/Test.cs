using Game.Pool;
using System.Collections.Generic;
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

        private int index;

        protected override void Awake()
        {
            items[0].localPosition = Vector3.zero;

            //items[1].localPosition = Utility.Vector.RelativePosition(60, 50).Vector3To2();

            items[1].localPosition = Utility.Vector.RotateClockwise(Vector3.zero, Vector3.right * 50, 60).Vector3To2();

            items[2].localPosition = Utility.Vector.RotateCounterclockwise(Vector3.zero, Vector3.right * 50, 60).Vector3To2();

            GameObject game = null;

            base.Awake();
        }

        private void OnEnable()
        {
           
        }

        private void OnDisable()
        {

        }

        private void OnDrawGizmos()
        {
            
        }

        private void Start()
        {
            var _ = StartAsync();
        }

        internal override void OnUpdate(float delta)
        {
            Debug.Log(delta);
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
            AudioManager.Instance.Play(AudioEnum.Music, null);
        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("OnClick")]
        public void OnClickContextMenu()
        {

        }

        public void OnClickButton()
        {

        }

        private async System.Threading.Tasks.Task StartAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Debuger.Log(Author.Test, "I'm fine!");
            });
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