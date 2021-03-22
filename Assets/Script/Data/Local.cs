using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public static class Local
    {
        public static bool GetBool(string key, bool value = false)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key) == 1;
            }
            return value;
        }

        public static int GetInt(string key, int value = 0)
        {
            return PlayerPrefs.GetInt(key, value);
        }

        public static float GetFloat(string key, float value = 0)
        {
            return PlayerPrefs.GetFloat(key, value);
        }

        public static Color GetColor(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string value = PlayerPrefs.GetString(key);

                if (!string.IsNullOrEmpty(value))
                {
                    if (ColorUtility.TryParseHtmlString(value, out Color color))
                    {
                        return color;
                    }
                }
            }
            return Color.white;
        }

        public static string GetString(string key, string value = null)
        {
            return PlayerPrefs.GetString(key, value);
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static void SetColor(string key, Color value)
        {
            PlayerPrefs.SetString(key, ColorUtility.ToHtmlStringRGBA(value));
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }

    public class LocalKey
    {
        public const string EffectStatus = "EffectStatus";
    }
}