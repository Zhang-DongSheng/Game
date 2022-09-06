using Game;
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

        private readonly List<string> keyword_begin = new List<string> { "(", "[", "{", "<", "（", "【", "《", "‘", "“", "\'", "\"", "「", "第" };

        private readonly List<string> keyword_among = new List<string> { "《", "》", "章", "节", "篇" };

        private readonly List<string> keyword_ending = new List<string> { ".", "。", "!", "！", "?", "？", "\'", "\"", "”", "’", ")", "]", "}", ">", "）", "】", "》", "」", "…" };

        private readonly List<string> keyword_character = new List<string>() { "/n", "/r", "/t", "\\n", "\\r", "\\t" };

        public void Convert(string input, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input)) return;

            if (!File.Exists(input)) return;

            string output = Utility.Path.New(input);

            if (File.Exists(output)) File.Delete(output);

            try
            {
                FileStream src = new FileStream(output, FileMode.CreateNew);

                using (FileStream dst = new FileStream(input, FileMode.Open))
                {
                    StreamWriter writer = new StreamWriter(src, encoding);

                    StreamReader reader = new StreamReader(dst, encoding);

                    string content = string.Empty, _temp = string.Empty;

                    while (!reader.EndOfStream)
                    {
                        _temp = Format(reader.ReadLine());

                        if (string.IsNullOrEmpty(_temp))
                        {
                            continue;
                        }

                        if (ParagraphStart(_temp))
                        {
                            if (!string.IsNullOrEmpty(content))
                            {
                                writer.WriteLine(content);
                                writer.Flush();
                            }
                            content = string.Empty;
                        }

                        content += _temp;

                        if (ParagraphEnd(content))
                        {
                            if (!string.IsNullOrEmpty(content))
                            {
                                writer.WriteLine(content);
                                writer.Flush();
                            }
                            content = string.Empty;
                        }
                    }
                    if (!string.IsNullOrEmpty(content))
                    {
                        writer.WriteLine(content);
                        writer.Flush();
                    }
                }
                src.Dispose();
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
            for (int i = 0; i < keyword_character.Count; i++)
            {
                content = content.Replace(keyword_character[i], null);
            }
            content = content.Trim();

            return content;
        }
    }
}