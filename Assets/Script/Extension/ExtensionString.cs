using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 追加
        /// </summary>
        public static string Append(this string self, params string[] values)
        {
            StringBuilder builder = new StringBuilder(self);

            for (int i = 0; i < values.Length; i++)
            {
                builder.Append(values[i]);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static List<T> SplitToList<T>(this string self, char separator)
        {
            if (string.IsNullOrEmpty(self)) return null;

            var list = new List<T>();

            var strs = self.Split(separator);

            foreach (var str in strs)
            {
                list.Add((T)Convert.ChangeType(str, typeof(T)));
            }
            return list;
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        public static string FirstToUpper(this string text)
        {
            if (text.Length > 0)
            {
                string first = text.Substring(0, 1);

                if (!string.IsNullOrEmpty(first))
                {
                    first = first.ToUpperInvariant();
                }
                text = first.Append(text.Remove(0, 1));
            }
            return text;
        }
        /// <summary>
        /// 判断是否存在双键数值
        /// </summary>
        public static bool RegexContains(this string source, string start, string end = null)
        {
            try
            {
                return new Regex(string.Format(@"{0}(.+?){1}", start, end)).IsMatch(source);
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
        public static List<string> RegexList(this string source, string start, string end)
        {
            try
            {
                Regex regex = new Regex(string.Format(@"{0}(.+?){1}", start, end));

                if (regex.IsMatch(source))
                {
                    List<string> list = new List<string>();

                    foreach (Match match in regex.Matches(source))
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
        public static bool IsNumber(this string self)
        {
            return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(self);
        }
        /// <summary>
        /// 判断字符串是否符合email格式
        /// </summary>
        public static bool IsEmail(this string self)
        {
            return Regex.IsMatch(self, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 判断字符串是否符合url格式
        /// </summary>
        public static bool IsURL(this string self)
        {
            return Regex.IsMatch(self, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }
        /// <summary>
        /// 判断字符串是否符合电话格式
        /// </summary>
        public static bool IsPhoneNumber(this string self)
        {
            return Regex.IsMatch(self, @"^(\(\d{3}\)|\d{3}-)?\d{7,8}$");
        }
        /// <summary>
        /// 判断字符串是否符合身份证号码格式
        /// </summary>
        public static bool IsIdentityNumber(this string self)
        {
            return Regex.IsMatch(self, @"^\d{17}[\d|X]|\d{15}$");
        }
        /// <summary>
        /// 转换为布尔型
        /// </summary>
        public static bool TryParseBool(this string self, out bool value)
        {
            return bool.TryParse(self, out value);
        }
        /// <summary>
        /// 转换为整型
        /// </summary>
        public static bool TryParseInt(this string self, out int value)
        {
            return int.TryParse(self, out value);
        }
        /// <summary>
        /// 转换为浮点型
        /// </summary>
        public static bool TryParseFloat(this string self, out float value)
        {
            return float.TryParse(self, out value);
        }
        /// <summary>
        /// 转换为高精度浮点型
        /// </summary>
        public static bool TryParseDouble(this string self, out double value)
        {
            return double.TryParse(self, out value);
        }
        /// <summary>
        /// 转换为长类型
        /// </summary>
        public static bool TryParseLong(this string self, out long value)
        {
            return long.TryParse(self, out value);
        }
        /// <summary>
        /// 转换为二维向量
        /// </summary>
        public static bool TryParseVector2(this string self, out Vector2 vector)
        {
            vector = new Vector2();

            string[] paramter = self.Split(',');

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
        public static bool TryParseVector3(this string self, out Vector3 vector)
        {
            vector = new Vector3();

            string[] paramter = self.Split(',');

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
        public static bool TryParseVector4(this string self, out Vector4 vector)
        {
            vector = new Vector4();

            string[] paramter = self.Split(',');

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
        public static bool TryParseQuaternion(this string self, Quaternion quaternion)
        {
            quaternion = new Quaternion();

            string[] paramter = self.Split(',');

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
        public static bool TryParseColor(this string self, out Color color)
        {
            return ColorUtility.TryParseHtmlString(self, out color);
        }
    }
}