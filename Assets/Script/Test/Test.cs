using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public RenewableAsset helper;

        private void Awake()
        {

        }

        private void Start()
        {
            helper.CreateAsset("android/picture/icon/icon_ceo", "ceo");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                //helper.Next(1);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
               // helper.Next(-1);
            }
        }

        private void OnCompleted()
        {
            Debug.LogError("10 last BG");

            string path = Application.dataPath + "/test.txt";

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("Begin");

                int number = 0;

                while (number++ < 100)
                {
                    writer.WriteLine(number);
                }
                writer.Flush();
            }
        }
    }
}