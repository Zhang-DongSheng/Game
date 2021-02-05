using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public Animation anima;

        private void Awake()
        {

        }

        private void Start()
        {
            //anima.RegisterBeginEvent(() =>
            //{
            //    Debug.LogError("接受成功");
            //});
            //anima.Play();

            //anima.RegisterEvent(1.2f,() =>
            //{
            //    Debug.LogError("接受成功");
            //});
            //anima.Play();

            anima.RegisterEndEvent(() =>
             {
                 Debug.LogError("接受成功");
             });
            anima.Play();
        }

        private void Update()
        {

        }
    }
}