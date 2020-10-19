using LitJson;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void SetTextWithEllipsis(this TextMeshProUGUI compontent, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            compontent.text = string.Empty;
        }
        else
        {
            compontent.text = content;

            compontent.ForceMeshUpdate();

            string text = compontent.GetParsedText();

            if (string.IsNullOrEmpty(text)) return;

            if (text.Length < 3) return;

            if (text.Length < content.Length)
            {
                text = content.Substring(0, text.Length - 3) + "...";
            }

            compontent.text = text;
        }
    }

    public static void SetTextWithEllipsis(this Text compontent, string content)
    {
        TextGenerator generator = new TextGenerator();

        RectTransform rectTransform = compontent.GetComponent<RectTransform>();

        TextGenerationSettings settings = compontent.GetGenerationSettings(rectTransform.rect.size);

        generator.Populate(content, settings);

        int count = generator.characterCountVisible;

        var text = content;

        if (content.Length > count)
        {
            text = content.Substring(0, count - 3) + "...";
        }

        compontent.text = text;
    }

    #region LitJson
    public static string GetString(this JsonData json, string key)
    {
        string result = string.Empty;

        if (json != null && json.ContainsKey(key) && json[key] != null)
        {
            result = json[key].ToString();
        }

        return result;
    }

    public static int GetInt(this JsonData json, string key)
    {
        int result = 0;

        if (json != null && json.ContainsKey(key) && json[key].IsInt)
        {
            int.TryParse(json[key].ToString(), out result);
        }

        return result;
    }

    public static float GetFloat(this JsonData json, string key)
    {
        float result = 0;

        if (json != null && json.ContainsKey(key))
        {
            float.TryParse(json[key].ToString(), out result);
        }

        return result;
    }

    public static long GetLong(this JsonData json, string key)
    {
        long result = 0;

        if (json != null && json.ContainsKey(key))
        {
            long.TryParse(json[key].ToString(), out result);
        }

        return result;
    }

    public static bool GetBool(this JsonData json, string key)
    {
        bool result = false;

        if (json != null && json.ContainsKey(key) && json[key].IsBoolean)
        {
            result = (bool)json[key];
        }

        return result;
    }

    public static byte GetByte(this JsonData json, string key)
    {
        byte result = 0;

        if (json != null && json.ContainsKey(key))
        {
            result = (byte)json[key];
        }

        return result;
    }

    public static JsonData GetJson(this JsonData json, string key)
    {
        if (json != null && json.ContainsKey(key))
        {
            return json[key];
        }
        return null;
    }

    public static T GetEnum<T>(this JsonData json, string key) where T : Enum
    {
        T result = default;

        if (json != null && json.ContainsKey(key))
        {
            if (json[key].IsInt)
            {
                int.TryParse(json[key].ToString(), out int type);

                int index = 0;

                foreach (T value in Enum.GetValues(typeof(T)))
                {
                    if (type == index++)
                    {
                        result = value;
                        break;
                    }
                }
            }
            else if (json[key].IsString)
            {
                string type = json[key].ToString();

                foreach (T value in Enum.GetValues(typeof(T)))
                {
                    if (type == value.ToString())
                    {
                        result = value;
                        break;
                    }
                }
            }
        }

        return result;
    }

    public static List<string> GetStrings(this JsonData json, string key, char separator = ',')
    {
        List<string> result = new List<string>();

        if (json != null && json.ContainsKey(key))
        {
            string[] _list = json[key].ToString().Split(separator);

            for (int i = 0; i < _list.Length; i++)
            {
                if (!string.IsNullOrEmpty(_list[i]))
                {
                    result.Add(_list[i]);
                }
            }
        }

        return result;
    }
    #endregion
}