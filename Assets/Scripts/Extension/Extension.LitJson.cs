using LitJson;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
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

        public static string[] GetStrings(this JsonData json, string key)
        {
            string[] result = null;

            if (json != null && json.ContainsKey(key))
            {
                if (json[key].IsArray)
                {
                    int count = json[key].Count;

                    result = new string[count];

                    for (int i = 0; i < count; i++)
                    {
                        result[i] = json[key][i].ToString();
                    }
                }
                else
                {
                    var content = json[key].ToString();

                    int length = content.Length;

                    content = content.Substring(1, length - 2);

                    var list = content.Split(',');

                    int count = list.Length;

                    result = new string[count];

                    for (int i = 0; i < count; i++)
                    {
                        result[i] = list[i];
                    }
                }
            }
            return result;
        }

        public static bool GetBool(this JsonData json, string key)
        {
            bool result = default;

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
                else if (json[key].IsString)
                {
                    result = json[key].ToString().ToLower() == "true";
                }
            }
            return result;
        }

        public static byte GetByte(this JsonData json, string key)
        {
            byte result = default;

            if (json != null && json.ContainsKey(key))
            {
                byte.TryParse(json[key].ToString(), out result);
            }
            return result;
        }

        public static char GetChar(this JsonData json, string key)
        {
            char result = default;

            if (json != null && json.ContainsKey(key))
            {
                char.TryParse(json[key].ToString(), out result);
            }
            return result;
        }

        public static short GetShort(this JsonData json, string key)
        {
            short result = default;

            if (json != null && json.ContainsKey(key))
            {
                short.TryParse(json[key].ToString(), out result);
            }
            return result;
        }

        public static int GetInt(this JsonData json, string key)
        {
            int result = default;

            if (json != null && json.ContainsKey(key) && json[key].IsInt)
            {
                result = (int)json[key];
            }
            return result;
        }

        public static int[] GetInts(this JsonData json, string key)
        {
            int[] result = null;

            if (json != null && json.ContainsKey(key))
            {
                if (json[key].IsArray)
                {
                    int count = json[key].Count;

                    result = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        int.TryParse(json[key][i].ToString(), out result[i]);
                    }
                }
                else
                {
                    var content = json[key].ToString();

                    int length = content.Length;

                    content = content.Substring(1, length - 2);

                    var list = content.Split(',');

                    int count = list.Length;

                    result = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        int.TryParse(list[i], out result[i]);
                    }
                }
            }
            return result;
        }

        public static uint GetUInt(this JsonData json, string key)
        {
            uint result = default;

            if (json != null && json.ContainsKey(key))
            {
                uint.TryParse(json[key].ToString(), out result);
            }
            return result;
        }

        public static float GetFloat(this JsonData json, string key)
        {
            float result = default;

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
                if (json[key].IsArray)
                {
                    int count = json[key].Count;

                    result = new float[count];

                    for (int i = 0; i < count; i++)
                    {
                        float.TryParse(json[key][i].ToString(), out result[i]);
                    }
                }
                else
                {
                    var content = json[key].ToString();

                    int length = content.Length;

                    content = content.Substring(1, length - 2);

                    var list = content.Split(',');

                    int count = list.Length;

                    result = new float[count];

                    for (int i = 0; i < count; i++)
                    {
                        float.TryParse(list[i], out result[i]);
                    }
                }
            }
            return result;
        }

        public static double GetDouble(this JsonData json, string key)
        {
            double result = 0;

            if (json != null && json.ContainsKey(key) && json[key].IsDouble)
            {
                if (json[key].IsLong)
                {
                    result = (double)json[key];
                }
                else
                {
                    double.TryParse(json[key].ToString(), out result);
                }
            }
            return result;
        }

        public static long GetLong(this JsonData json, string key)
        {
            long result = 0;

            if (json != null && json.ContainsKey(key))
            {
                if (json[key].IsLong)
                {
                    result = (long)json[key];
                }
                else
                {
                    long.TryParse(json[key].ToString(), out result);
                }
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
            Vector2 value = default;

            if (json != null && json.ContainsKey(key))
            {
                string content = json[key].ToString();

                content = content.Substring(1, content.Length - 2);

                string[] list = content.Split(',');

                if (list.Length == 2)
                {
                    float.TryParse(list[0], out value.x);

                    float.TryParse(list[1], out value.y);
                }
            }
            return value;
        }

        public static Vector3 GetVector3(this JsonData json, string key)
        {
            Vector3 value = default;

            if (json != null && json.ContainsKey(key))
            {
                string content = json[key].ToString();

                content = content.Substring(1, content.Length - 2);

                string[] list = content.Split(',');

                if (list.Length == 3)
                {
                    float.TryParse(list[0], out value.x);

                    float.TryParse(list[1], out value.y);

                    float.TryParse(list[2], out value.z);
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

        public static UIntPair GetUIntPair(this JsonData json, string key)
        {
            UIntPair result = default;

            if (json != null && json.ContainsKey(key) && json[key].IsArray)
            {
                result = new UIntPair()
                {
                    x = (uint)json[key][0],
                    y = (uint)json[key][1]
                };
            }
            return result;
        }

        public static List<UIntPair> GetUIntPairs(this JsonData json, string key)
        {
            List<UIntPair> result = new List<UIntPair>();

            if (json != null && json.ContainsKey(key) && json[key].IsArray)
            {
                for (int i = 0; i < json[key].Count; i++)
                {
                    result.Add(new UIntPair()
                    {
                        x = (uint)json[key][i][0],
                        y = (uint)json[key][i][1],
                    });
                }
            }
            return result;
        }

        public static T GetType<T>(this JsonData json) where T : class
        {
            if (json == null) return null;

            T result = Activator.CreateInstance<T>();

            FieldInfo[] fields = result.GetType().GetFields();

            foreach (var field in fields)
            {
                string key = field.Name;

                if (field.FieldType == typeof(string))
                {
                    field.SetValue(result, json.GetString(key));
                }
                else if (field.FieldType == typeof(bool))
                {
                    field.SetValue(result, json.GetBool(key));
                }
                else if (field.FieldType == typeof(byte))
                {
                    field.SetValue(result, json.GetByte(key));
                }
                else if (field.FieldType == typeof(char))
                {
                    field.SetValue(result, json.GetChar(key));
                }
                else if (field.FieldType == typeof(short))
                {
                    field.SetValue(result, json.GetShort(key));
                }
                else if (field.FieldType == typeof(int))
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
                else if (field.FieldType == typeof(double))
                {
                    field.SetValue(result, json.GetDouble(key));
                }
                else if (field.FieldType == typeof(long))
                {
                    field.SetValue(result, json.GetLong(key));
                }
                else if (field.FieldType == typeof(UIntPair))
                {
                    field.SetValue(result, json.GetUIntPair(key));
                }
                else if (field.FieldType == typeof(List<UIntPair>))
                {
                    field.SetValue(result, json.GetUIntPairs(key));
                }
                else
                {
                    Debuger.LogWarning(Author.Data, $"Can't convert the type of {field.FieldType}! Please add converter");
                }
            }
            return result;
        }
    }
}