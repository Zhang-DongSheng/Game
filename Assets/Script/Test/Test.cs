using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public UISegment helper;

        private void Awake()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < 5 * 60 + 2; i++)
            {
                list.Add(i);
            }

            helper.callback = (item, data) =>
            {
                item.GetComponentInChildren<Text>().text = data.ToString();
            };
                


            helper.Refresh(list);
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

            //anima.RegisterEndEvent(() =>
            // {
            //     Debug.LogError("接受成功");
            // });
            //anima.Play();
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
        }
    }
}