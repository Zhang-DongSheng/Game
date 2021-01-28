using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Text))]
    public class TextRunningHorseLamp : MonoBehaviour
    {
        [SerializeField] private Text compontent;

        [SerializeField] private int length;

        [SerializeField] private float interval;

        private string content, value;

        private char[] values;

        private int index, count;

        private float timer;

        private void Awake()
        {
            if (compontent == null)
                compontent = GetComponent<Text>();
        }

        private void Update()
        {
            if (count == 0) return;

            timer += Time.deltaTime;

            if (timer > interval)
            {
                timer = 0;

                index++;

                if (index >= count) index = 0;

                value = string.Empty;

                for (int i = 0; i < values.Length; i++)
                {
                    if (i >= index && i < index + length)
                    {
                        value += values[i];
                    }
                }

                compontent.text = value;
            }
        }

        public string Text
        {
            get { return content; }
            set
            {
                content = value;

                if (string.IsNullOrEmpty(content))
                {
                    index = 0; count = 0;
                }
                else
                {
                    index = 0;

                    count = content.Length;

                    values = new char[count];

                    for (int i = 0; i < count; i++)
                    {
                        values[i] = content[i];
                    }
                }
            }
        }
    }
}
