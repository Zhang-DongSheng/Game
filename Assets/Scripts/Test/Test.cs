using UnityEngine;
using UnityEngine.Audio;

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
        
        public string content;

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

            //UnityGameObject unity = new GameObject("XX");


            GameObject unity = new GameObject("XX");

            string json = JsonUtility.ToJson(unity.transform.position);

            Debuger.Log(Author.Test, json);
        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick()
        {
            AudioManager.Instance.PlayMusic("xxxx.mp3", false);
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