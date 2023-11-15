using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private static readonly StringBuilder builder = new StringBuilder();
        /// <summary>
        /// 追加
        /// </summary>
        public static string Append(this string str, params string[] values)
        {
            builder.Clear(); builder.Append(str);

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
        /// 部分大写[65-90]
        /// </summary>
        public static string SubToUpper(this string str, int index = 0, int count = 1)
        {
            builder.Clear();

            char value;

            for (int i = 0; i < str.Length; i++)
            {
                value = str[i];

                if (i >= index && count-- > 0)
                {
                    if (value >= Utility.ASCii.LOWERCASEMIN && value <= Utility.ASCii.LOWERCASEMAX)
                    {
                        value = Convert.ToChar(value - 32);
                    }
                }
                builder.Append(value);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 部分小写[97-122]
        /// </summary>
        public static string SubToLower(this string str, int index = 0, int count = 1)
        {
            builder.Clear();

            char value;

            for (int i = 0; i < str.Length; i++)
            {
                value = str[i];

                if (i >= index && count-- > 0)
                {
                    if (value >= Utility.ASCii.UPPERCASEMIN && value <= Utility.ASCii.UPPERCASEMAX)
                    {
                        value = Convert.ToChar(value + 32);
                    }
                }
                builder.Append(value);
            }
            return builder.ToString();
        }

        #region Regex
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
        /// 判断字符串是否存在双键数值
        /// </summary>
        public static bool IsContainsKey(this string str, string start, string end = null)
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
        /// 获取字符串中包含的参数
        /// </summary>
        public static List<string> GetParameters(this string str, string start, string end)
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
        #endregion

        #region Parse
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
        /// 转换为时间
        /// </summary>
        public static bool TryParseDateTime(this string str, out DateTime time)
        {
            if (DateTime.TryParse(str, out time))
            {
                return true;
            }
            else
            {
                time = DateTime.Now; return false;
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
        /// <summary>
        /// 转换为整型数组
        /// </summary>
        public static bool TryParseArrayInt(this string str, out int[] array)
        {
            bool result = true;

            string[] paramter = str.Split(',');

            int length = paramter.Length;

            array = new int[length];

            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrEmpty(paramter[i]) || !int.TryParse(paramter[i], out array[i]))
                {
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换为浮点型数组
        /// </summary>
        public static bool TryParseArrayFloat(this string str, out float[] array)
        {
            bool result = true;

            string[] paramter = str.Split(',');

            int length = paramter.Length;

            array = new float[length];

            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrEmpty(paramter[i]) || !float.TryParse(paramter[i], out array[i]))
                {
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 转换为长类型数组
        /// </summary>
        public static bool TryParseArrayLong(this string str, out long[] array)
        {
            bool result = true;

            string[] paramter = str.Split(',');

            int length = paramter.Length;

            array = new long[length];

            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrEmpty(paramter[i]) || !long.TryParse(paramter[i], out array[i]))
                {
                    result = false;
                }
            }
            return result;
        }
        #endregion

        #region Rich Text
        /// <summary>
        /// 富文本文字风格
        /// </summary>
        public static string RichText(this string str, FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Bold:
                    return string.Format("<b>{0}</b>", str);
                case FontStyle.Italic:
                    return string.Format("<i>{0}</i>", str);
                case FontStyle.BoldAndItalic:
                    return string.Format("<b><i>{0}</i></b>", str);
                default: return str;
            }
        }
        /// <summary>
        /// 富文本文字大小
        /// </summary>
        public static string RichTextSize(this string str, int size)
        {
            return string.Format("<size={0}>{1}</size>", size, str);
        }
        /// <summary>
        /// 富文本文字颜色
        /// </summary>
        public static string RichTextColor(this string str, Color color)
        {
            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), str);
        }
        /// <summary>
        /// 富文本换行
        /// </summary>
        public static string RichTextLine(this string str)
        {
            return string.Format("{0}\n", str);
        }
        #endregion

        #region ToString
        /// <summary>
        /// 显示数量
        /// </summary>
        public static string ToStringNumber(this int number, int digit = 2)
        {
            if (number < 1000)
            {
                value = number;
            }
            else if (number < 1000000)
            {
                value = number / 1000; unit = UnitQuantity[1];
            }
            else
            {
                value = number / 1000000; unit = UnitQuantity[2];
            }
            return string.Format("{0}{1}", Math.Round(value, digit), unit);
        }
        /// <summary>
        /// 显示大小
        /// </summary>
        public static string ToStringSize(this long number, int digit = 2)
        {
            if (number <= Kilobyte)
            {
                return $"{number} {UnitByte[0]}";
            }
            int pow = Math.Min((int)Math.Floor(Math.Log(number, Kilobyte)), UnitByte.Length - 1);

            return $"{Math.Round(number / Math.Pow(Kilobyte, pow), digit)} {UnitByte[pow]}";
        }
        /// <summary>
        /// 显示大小
        /// </summary>
        public static string ToStringSize(this int number, int digit = 2)
        {
            if (number <= Kilobyte)
            {
                return $"{number} {UnitByte[0]}";
            }
            int pow = Math.Min((int)Math.Floor(Math.Log(number, Kilobyte)), UnitByte.Length - 1);

            return $"{Math.Round(number / Math.Pow(Kilobyte, pow), digit)} {UnitByte[pow]}";
        }
        /// <summary>
        /// 显示时间
        /// </summary>
        public static string ToStringTime(this int seconds)
        {
            TimeSpan span = TimeSpan.FromSeconds(seconds);

            if (span.Days > 0)
            {
                value = span.Days; unit = UnitTime[3];
            }
            else if (span.Hours > 0)
            {
                value = span.Hours; unit = UnitTime[2];
            }
            else if (span.Minutes > 0)
            {
                value = span.Minutes; unit = UnitTime[1];
            }
            else
            {
                value = seconds; unit = UnitTime[0];
            }
            return string.Format("{0}{1}", value, unit);
        }
        #endregion
    }
}