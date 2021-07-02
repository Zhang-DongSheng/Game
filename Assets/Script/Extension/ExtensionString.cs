using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
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
        /// <param name="separator">关键字</param>
        public static List<T> SplitToList<T>(this string source, char separator)
        {
            if (string.IsNullOrEmpty(source)) return null;

            var list = new List<T>();

            var strs = source.Split(separator);

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
        /// <param name="start">前键</param>
        /// <param name="end">后键</param>
        /// <returns></returns>
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
        /// <param name="start">前键</param>
        /// <param name="end">后键</param>
        /// <returns></returns>
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
    }
}