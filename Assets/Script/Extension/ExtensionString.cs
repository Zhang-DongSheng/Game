using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        static private readonly StringBuilder builder = new StringBuilder();
        /// <summary>
        /// 追加
        /// </summary>
        public static string Append(this string str, params string[] values)
        {
            StringBuilder builder = new StringBuilder(str);

            for (int i = 0; i < values.Length; i++)
            {
                builder.Append(values[i]);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static List<T> SplitToList<T>(this string str, char separator)
        {
            if (string.IsNullOrEmpty(str)) return null;

            var list = new List<T>();

            var strs = str.Split(separator);

            foreach (var _str in strs)
            {
                if (_str.TryParse(out T item))
                {
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        public static string FirstToUpper(this string str)
        {
            /* * [97-122] -> [65-90] * */ 

            if (str.Length > 0)
            {
                char first = str[0];

                if (first > 96 && first < 123)
                {
                    first = Convert.ToChar(first - 32);

                    str = string.Format("{0}{1}", first, str.Remove(0, 1));
                }
            }
            return str;
        }
        /// <summary>
        /// 判断是否存在双键数值
        /// </summary>
        public static bool RegexContains(this string str, string start, string end = null)
        {
            try
            {
                return new Regex(string.Format(@"{0}(.+?){1}", start, end)).IsMatch(str);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }
        /// <summary>
        /// 获取双键数值
        /// </summary>
        public static List<string> RegexList(this string str, string start, string end)
        {
            try
            {
                Regex regex = new Regex(string.Format(@"{0}(.+?){1}", start, end));

                if (regex.IsMatch(str))
                {
                    List<string> list = new List<string>();

                    foreach (Match match in regex.Matches(str))
                    {
                        list.Add(match.Groups[1].Value);
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return null;
        }
        /// <summary>
        /// 判断字符串是否是数值型
        /// </summary>
        public static bool IsNumber(this string str)
        {
            return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(str);
        }
        /// <summary>
        /// 判断字符串是否符合email格式
        /// </summary>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 判断字符串是否符合url格式
        /// </summary>
        public static bool IsURL(this string str)
        {
            return Regex.IsMatch(str, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }
        /// <summary>
        /// 判断字符串是否符合电话格式
        /// </summary>
        public static bool IsPhoneNumber(this string str)
        {
            return Regex.IsMatch(str, @"^(\(\d{3}\)|\d{3}-)?\d{7,8}$");
        }
        /// <summary>
        /// 判断字符串是否符合身份证号码格式
        /// </summary>
        public static bool IsIdentityNumber(this string str)
        {
            return Regex.IsMatch(str, @"^\d{17}[\d|X]|\d{15}$");
        }
        /// <summary>
        /// 转换为泛型
        /// </summary>
        public static bool TryParse<T>(this string str, out T value)
        {
            bool result = false;

            try
            {
                value = (T)Convert.ChangeType(str, typeof(T));

                if (!Convert.IsDBNull(value))
                {
                    result = true;
                }
            }
            catch
            {
                value = default;
            }
            return result;
        }
        /// <summary>
        /// 转换为布尔型
        /// </summary>
        public static bool TryParseBool(this string str, out bool value, bool define = false)
        {
            if (bool.TryParse(str, out value))
            {
                return true;
            }
            else
            {
                value = define; return false;
            }
        }
        /// <summary>
        /// 转换为整型
        /// </summary>
        public static bool TryParseInt(this string str, out int value, int define = 0)
        {
            if (int.TryParse(str, out value))
            {
                return true;
            }
            else
            {
                value = define; return false;
            }
        }
        /// <summary>
        /// 转换为浮点型
        /// </summary>
        public static bool TryParseFloat(this string str, out float value, float define = 0)
        {
            if (float.TryParse(str, out value))
            {
                return true;
            }
            else
            {
                value = define; return false;
            }
        }
        /// <summary>
        /// 转换为高精度浮点型
        /// </summary>
        public static bool TryParseDouble(this string str, out double value, double define = 0)
        {
            if (double.TryParse(str, out value))
            {
                return true;
            }
            else
            {
                value = define; return false;
            }
        }
        /// <summary>
        /// 转换为长类型
        /// </summary>
        public static bool TryParseLong(this string str, out long value, long define = 0)
        {
            if (long.TryParse(str, out value))
            {
                return true;
            }
            else
            {
                value = define; return false;
            }
        }
        /// <summary>
        /// 转换为二维向量
        /// </summary>
        public static bool TryParseVector2(this string str, out Vector2 vector)
        {
            vector = new Vector2();

            string[] paramter = str.Split(',');

            if (paramter.Length == 2)
            {
                return float.TryParse(paramter[0], out vector.x)
                    && float.TryParse(paramter[1], out vector.y);
            }
            return false;
        }
        /// <summary>
        /// 转换为三维向量
        /// </summary>
        public static bool TryParseVector3(this string str, out Vector3 vector)
        {
            vector = new Vector3();

            string[] paramter = str.Split(',');

            if (paramter.Length == 3)
            {
                return float.TryParse(paramter[0], out vector.x)
                    && float.TryParse(paramter[1], out vector.y)
                    && float.TryParse(paramter[2], out vector.z);
            }
            return false;
        }
        /// <summary>
        /// 转换为四维向量
        /// </summary>
        public static bool TryParseVector4(this string str, out Vector4 vector)
        {
            vector = new Vector4();

            string[] paramter = str.Split(',');

            if (paramter.Length == 4)
            {
                return float.TryParse(paramter[0], out vector.x)
                    && float.TryParse(paramter[1], out vector.y)
                    && float.TryParse(paramter[2], out vector.z)
                    && float.TryParse(paramter[3], out vector.w);
            }
            return false;
        }
        /// <summary>
        /// 转换为四元数
        /// </summary>
        public static bool TryParseQuaternion(this string str, Quaternion quaternion)
        {
            quaternion = new Quaternion();

            string[] paramter = str.Split(',');

            if (paramter.Length == 4)
            {
                return float.TryParse(paramter[0], out quaternion.x)
                    && float.TryParse(paramter[1], out quaternion.y)
                    && float.TryParse(paramter[2], out quaternion.z)
                    && float.TryParse(paramter[3], out quaternion.w);
            }
            return false;
        }
        /// <summary>
        /// 转换为颜色值
        /// </summary>
        public static bool TryParseColor(this string str, out Color color)
        {
            if (string.IsNullOrEmpty(str))
            {
                color = Color.clear; return false;
            }
            if (!str.StartsWith("#")) str = string.Format("#{0}", str);

            return ColorUtility.TryParseHtmlString(str, out color);
        }
    }
}