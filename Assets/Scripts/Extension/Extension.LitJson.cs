using LitJson;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static string GetString(this JsonData json, string key)
        {
            string result = string.Empty;

            if (json != null && json.ContainsKey(key) && json[key] != null)
            {
                result = json[key].ToString();
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

        public static int GetInt(this JsonData json, string key)
        {
            int result = 0;

            if (json != null && json.ContainsKey(key) && json[key].IsInt)
            {
                int.TryParse(json[key].ToString(), out result);
            }
            return result;
        }

        public static int[] GetInts(this JsonData json, string key)
        {
            int[] result = null;

            if (json != null && json.ContainsKey(key))
            {
                string content = json[key].ToString().Trim('[', ']');

                string[] values = content.Split(',');

                int count = values.Length;

                result = new int[count];

                for (int i = 0; i < count; i++)
                {
                    int.TryParse(values[i], out result[i]);
                }
            }
            return result;
        }

        public static uint GetUInt(this JsonData json, string key)
        {
            uint result = 0;

            if (json != null && json.ContainsKey(key) && json[key].IsInt)
            {
                uint.TryParse(json[key].ToString(), out result);
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

        public static float[] GetFloats(this JsonData json, string key)
        {
            float[] result = null;

            if (json != null && json.ContainsKey(key))
            {
                string content = json[key].ToString().Trim('[', ']');

                string[] values = content.Split(',');

                int count = values.Length;

                result = new float[count];

                for (int i = 0; i < count; i++)
                {
                    float.TryParse(values[i], out result[i]);
                }
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

            if (json != null && json.ContainsKey(key))
            {
                if (json[key].IsBoolean)
                {
                    result = (bool)json[key];
                }
                else if (json[key].IsInt)
                {
                    result = (int)json[key] != 0;
                }
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

        public static Vector2 GetVector2(this JsonData json, string key)
        {
            Vector2 value = new Vector2();

            if (json != null && json.ContainsKey(key))
            {
                string content = json[key].ToString().Trim('(', ')');

                string[] values = content.Split(',');

                if (values.Length == 2)
                {
                    float.TryParse(values[0], out value.x);

                    float.TryParse(values[1], out value.y);
                }
            }
            return value;
        }

        public static Vector3 GetVector3(this JsonData json, string key)
        {
            Vector3 value = new Vector3();

            if (json != null && json.ContainsKey(key))
            {
                string content = json[key].ToString().Trim('(', ')');

                string[] values = content.Split(',');

                if (values.Length == 3)
                {
                    float.TryParse(values[0], out value.x);

                    float.TryParse(values[1], out value.y);

                    float.TryParse(values[2], out value.z);
                }
            }
            return value;
        }


        public static Color GetColor(this JsonData json, string key, float alpha = 1)
        {
            Color color = Color.white;

            if (json != null && json.ContainsKey(key))
            {
                string value = json[key].ToString();

                if (string.IsNullOrEmpty(value))
                {
                    color.a = alpha;
                }
                else
                {
                    if (value.Substring(0, 1) != "#")
                    {
                        value = "#" + value;
                    }
                    ColorUtility.TryParseHtmlString(value, out color);
                }
            }
            return color;
        }

        public static T GetType<T>(this JsonData json) where T : class
        {
            if (json == null) return null;

            T result = Activator.CreateInstance<T>();

            FieldInfo[] fields = result.GetType().GetFields();

            foreach (var field in fields)
            {
                string key = field.Name.ToLower();

                if (field.FieldType == typeof(int))
                {
                    field.SetValue(result, json.GetInt(key));
                }
                else if (field.FieldType == typeof(int[]))
                {
                    field.SetValue(result, json.GetInts(key));
                }
                else if (field.FieldType == typeof(uint))
                {
                    field.SetValue(result, json.GetUInt(key));
                }
                else if (field.FieldType == typeof(float))
                {
                    field.SetValue(result, json.GetFloat(key));
                }
                else if (field.FieldType == typeof(float[]))
                {
                    field.SetValue(result, json.GetFloats(key));
                }
                else if (field.FieldType == typeof(long))
                {
                    field.SetValue(result, json.GetLong(key));
                }
                else if (field.FieldType == typeof(bool))
                {
                    field.SetValue(result, json.GetBool(key));
                }
                else if (field.FieldType == typeof(string))
                {
                    field.SetValue(result, json.GetString(key));
                }
                else
                {

                }
            }
            return result;
        }
    }
}