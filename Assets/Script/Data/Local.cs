using System;
using System.IO;

namespace UnityEngine
{
    public static class Local
    {
        #region PlayerPrefs
        public static T GetValue<T>(string key, T define = default)
        {
            T result = define;

            if (PlayerPrefs.HasKey(key))
            {
                switch (typeof(T).ToString())
                {
                    case "System.Boolean":
                        result = (T)(object)(PlayerPrefs.GetInt(key) == 1);
                        break;
                    case "System.Int32":
                        result = (T)(object)PlayerPrefs.GetInt(key);
                        break;
                    case "System.Single":
                        result = (T)(object)PlayerPrefs.GetFloat(key);
                        break;
                    case "System.Double":
                        {
                            string value = PlayerPrefs.GetString(key);

                            if (double.TryParse(value, out double _value))
                            {
                                result = (T)(object)_value;
                            }
                        }
                        break;
                    case "System.Int64":
                        {
                            string value = PlayerPrefs.GetString(key);

                            if (long.TryParse(value, out long _value))
                            {
                                result = (T)(object)_value;
                            }
                        }
                        break;
                    case "System.String":
                        result = (T)(object)PlayerPrefs.GetString(key);
                        break;
                    case "UnityEngine.Vector2":
                        {

                        }
                        break;
                    case "UnityEngine.Vector3":
                        {

                        }
                        break;
                    case "UnityEngine.Color":
                        {
                            string value = PlayerPrefs.GetString(key);

                            if (!string.IsNullOrEmpty(value))
                            {
                                if (!value.StartsWith("#")) value = string.Format("#{0}", value);

                                if (ColorUtility.TryParseHtmlString(value, out Color color))
                                {
                                    result = (T)(object)color;
                                }
                            }
                        }
                        break;
                    default:
                        Debug.LogWarningFormat("The Type of {0} is not support!", typeof(T));
                        break;
                }
            }
            return result;
        }

        public static void SetValue<T>(string key, T value)
        {
            switch (typeof(T).ToString())
            {
                case "System.Boolean":
                    PlayerPrefs.SetInt(key, Convert.ToBoolean(value) ? 1 : 0);
                    break;
                case "System.Int32":
                    PlayerPrefs.SetInt(key, Convert.ToInt32(value));
                    break;
                case "System.Single":
                    PlayerPrefs.SetFloat(key, Convert.ToSingle(value));
                    break;
                case "System.Int64":
                case "System.Double":
                case "System.String":
                    PlayerPrefs.SetString(key, value.ToString());
                    break;
                case "UnityEngine.Vector2":
                    {

                    }
                    break;
                case "UnityEngine.Vector3":
                    {

                    }
                    break;
                case "UnityEngine.Color":
                    {
                        try
                        {
                            Color color = (Color)(object)value;

                            PlayerPrefs.SetString(key, ColorUtility.ToHtmlStringRGBA(color));
                        }
                        catch
                        {

                        }
                    }
                    break;
                default:
                    PlayerPrefs.SetString(key, value.ToString());
                    break;
            }
        }

        public static void Remove(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        #endregion

        #region File
        public static string Read(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }

        public static void Write(string path, params string[] content)
        {
            string folder = Path.GetDirectoryName(path);

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                for (int i = 0; i < content.Length; i++)
                {
                    writer.WriteLine(content[i]);
                }
                writer.Flush(); writer.Dispose();
            }
        }

        public static void WriteAppend(string path, params string[] content)
        {
            if (File.Exists(path))
            {
                File.AppendAllLines(path, content);
            }
            else
            {
                Write(path, content);
            }
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion
    }

    public class LocalKey
    {
        public const string EffectStatus = "EffectStatus";
    }
}