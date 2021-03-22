using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public TimeHelper helper;

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
                //slider.Play(Random.Range(0, 1f));

                helper.onValueChanged.AddListener((value) =>
                {
                    GetComponent<Text>().text = value.ToString();
                });

                helper.Start(60);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                //slider.Play(Random.Range(0, 1f));

                TimeManager.Instance.Register("Test", new TimeTask()
                {
                    timer = Time.time + 5,
                    interval = 5,
                    loop = true,
                    callBack = OnCompleted
                });
            }
        }

        private void OnCompleted()
        {
            Debug.LogError("10 last BG");
        }
    }
}