using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Encrypt;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public InputField inputField;

        public Button button;

        private string value;

        private Game.Network.Client network;

        private void Awake()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);

            button.onClick.AddListener(OnSubmit);

            network = new Game.Network.Client("127.0.0.1", 88);

            network.onReceive = (message) =>
            {
                  Debug.LogError(message);
            };
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                network.Close();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                network.Send("Try");
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
            network.Send(value);
        }
    }
}