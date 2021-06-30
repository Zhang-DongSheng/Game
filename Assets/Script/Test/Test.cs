using Game;
using Game.Network;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public InputField inputField;

        public Button button;

        private string value;

        private void Awake()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);

            button.onClick.AddListener(OnSubmit);

            NetworkManager.Instance.Connection();
        }

        private void OnEnable()
        {
            NetworkEventManager.Register(NetworkEventKey.Test, OnReceiveTest);
        }

        private void OnDisable()
        {
            NetworkEventManager.Unregister(NetworkEventKey.Test, OnReceiveTest);
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NetworkManager.Instance.Close();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                NetworkManager.Instance.Send("Try");
            }
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {

        }

        private void OnValueChanged(string value)
        {
            this.value = value;
        }

        private void OnSubmit()
        {
            NetworkManager.Instance.Send(value);
        }

        private void OnReceiveTest(NetworkEventHandle handle)
        {
            Debug.LogError(handle.content);
        }
    }
}