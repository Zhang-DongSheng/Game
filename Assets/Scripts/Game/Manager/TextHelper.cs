using Data;
using UnityEngine.UI;

namespace Game
{
    public static class TextHelper
    {
        private static DataText dictionary;

        public static void SetString(Text component, int value)
        {
            SetString(component, string.Format("{0}", value), false);
        }

        public static void SetString(Text component, float value)
        {
            SetString(component, string.Format("{0}", value), false);
        }

        public static void SetString(Text component, long value)
        {
            SetString(component, string.Format("{0}", value), false);
        }

        public static void SetString(Text component, string content, bool language = true)
        {
            if (language)
            {
                if (dictionary != null)
                {
                    SetString(component, dictionary.Get(content), false);
                }
                else
                {
                    dictionary = DataManager.Instance.Load<DataText>();

                    if (dictionary != null)
                    {
                        SetString(component, dictionary.Get(content), false);
                    }
                    else
                    {
                        SetString(component, content, false);
                    }
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

        public static void SetStringFormat(Text component, string format, params string[] parameter)
        {
            string content = string.Format(format, parameter);

            SetString(component, content, false);
        }
    }
}