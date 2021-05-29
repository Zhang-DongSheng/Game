using System.Collections.Generic;
using System.IO;
using UnityEngine;
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

        public static void Startover(params string[] paramters)
        {
            for (int i = 0; i < paramters.Length; i++)
            {
                Debug.LogError(paramters[i]);
            }
        }
    }
}