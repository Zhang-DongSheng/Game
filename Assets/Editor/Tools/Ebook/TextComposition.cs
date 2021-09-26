using Game.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor.Ebook
{
    public class TextComposition
    {
        public Action onSuccess;

        public Action<string> onFail;

        private const int MAXNUMBER = 500;

        private readonly List<string> keyword_begin = new List<string> { "(", "[", "{", "<", "£¨", "¡¾", "¡¶", "¡®", "¡°", "\'", "\"", "¡¸", "µÚ" };

        private readonly List<string> keyword_among = new List<string> { "¡¶", "¡·", "ÕÂ", "½Ú", "Æª" };

        private readonly List<string> keyword_ending = new List<string> { ".", "¡£", "!", "£¡", "?", "£¿", "\'", "\"", "¡±", "¡¯", ")", "]", "}", ">", "£©", "¡¿", "¡·", "¡¹", "¡­" };

        public void Convert(string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source)) return;

            if (!File.Exists(source)) return;

            string document = FileUtils.New(source);

            if (File.Exists(document)) File.Delete(document);

            try
            {
                FileStream stream = new FileStream(document, FileMode.CreateNew);

                using (FileStream fs = new FileStream(source, FileMode.Open))
                {
                    StreamWriter sw = new StreamWriter(stream, encoding);
                    StreamReader sr = new StreamReader(fs, encoding);

                    string content = string.Empty, _temp = string.Empty;

                    while (!sr.EndOfStream)
                    {
                        _temp = Format(sr.ReadLine());

                        if (string.IsNullOrEmpty(_temp))
                        {
                            continue;
                        }

                        if (ParagraphStart(_temp))
                        {
                            if (!string.IsNullOrEmpty(content))
                            {
                                sw.WriteLine(content);
                                sw.Flush();
                            }
                            content = string.Empty;
                        }

                        content += _temp;

                        if (ParagraphEnd(content))
                        {
                            if (!string.IsNullOrEmpty(content))
                            {
                                sw.WriteLine(content);
                                sw.Flush();
                            }
                            content = string.Empty;
                        }
                    }
                    if (!string.IsNullOrEmpty(content))
                    {
                        sw.WriteLine(content);
                        sw.Flush();
                    }
                }
                stream.Dispose();
            }
            catch (Exception e)
            {
                onFail?.Invoke(e.Message);
            }
            finally
            {
                onSuccess?.Invoke();
            }
        }

        private bool ParagraphStart(string content)
        {
            if (string.IsNullOrEmpty(content)) return false;

            for (int i = 0; i < keyword_begin.Count; i++)
            {
                if (content.StartsWith(keyword_begin[i]))
                {
                    return true;
                }
            }

            for (int i = 0; i < keyword_among.Count; i++)
            {
                if (content.Contains(keyword_among[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ParagraphEnd(string content)
        {
            if (string.IsNullOrEmpty(content)) return false;

            if (content.Length > MAXNUMBER)
            {
                Debug.LogWarningFormat("More than {0} words, content : {1}", MAXNUMBER, content);
                return true;
            }
            else
            {
                for (int i = 0; i < keyword_among.Count; i++)
                {
                    if (content.Contains(keyword_among[i]))
                    {
                        return true;
                    }
                }

                for (int i = 0; i < keyword_ending.Count; i++)
                {
                    if (content.EndsWith(keyword_ending[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string Format(string content)
        {
            content = content.Replace("/n", "");
            content = content.Replace("/r", "");
            content = content.Replace("/t", "");
            content = content.Trim();

            return content;
        }
    }
}