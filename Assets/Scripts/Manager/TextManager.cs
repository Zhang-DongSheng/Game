using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Game
{
    public class TextManager : Singleton<TextManager>
    {
        private Dictionary dictionary;

        public void SetString(Text component, int value)
        {
            SetString(component, string.Format("{0}", value), false);
        }

        public void SetString(Text component, float value)
        {
            SetString(component, string.Format("{0}", value), false);
        }

        public void SetString(Text component, long value)
        {
            SetString(component, string.Format("{0}", value), false);
        }

        public void SetString(Text component, string content, bool language = true)
        {
            if (language)
            {
                if (dictionary != null)
                {
                    SetString(component, dictionary.Word(content), false);
                }
                else
                {
                    DataManager.Instance.LoadAsync<DataText>(asset =>
                    {
                        dictionary = asset.dictionaries.Find(x => x.language == Language.Chinese);

                        SetString(component, dictionary.Word(content), false);
                    });
                }
            }
            else
            {
                if (component != null)
                {
                    component.text = content;
                }
            }
        }

        public void SetStringFormat(Text component, string format, params string[] parameter)
        {
            string content = string.Format(format, parameter);

            SetString(component, content, false);
        }
    }
}