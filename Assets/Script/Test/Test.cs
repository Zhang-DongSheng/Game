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
        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {

        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            string v1 = "i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!i am a good boy! please give me money!";

            string v2 = FileEncrypt.Encrypt(v1);

            string v3 = FileEncrypt.Decrypt(v2);

            //string v2 = FileEncrypt.DESEncrypt(v1);

            //string v3 = FileEncrypt.DESDecrypt(v2);

            Debug.Log("v1 : " + v1);

            Debug.Log("v2 : " + v2);

            Debug.Log("v3 : " + v3);
        }
    }
}