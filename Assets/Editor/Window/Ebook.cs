using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor.Window
{
    class Ebook : EditorWindow
    {
        enum CodeType
        {
            Default,
            ANSI,
            Unicode,
            UTF8,
            UTF32,
            Chinese,
        }

        private readonly string[] text_title = new string[] { "主页", "设置", "其他" };

        private readonly string KEY = "EBOOKPATH";

        private readonly EbookConvert ebook = new EbookConvert();

        #region Temporary Variable
        private int index_view;

        private Vector2 scroll_book;

        private CodeType value_code;

        private int value_number = 1000;

        private string value_input;
        #endregion

        #region Param
        private Encoding encoding = Encoding.Default;

        private int lineCount = 1000;

        private bool select = false;

        private readonly List<Book> list = new List<Book>();
        #endregion

        [MenuItem("Extra/Ebook")]
        private static void Open()
        {
            EditorWindow window = EditorWindow.GetWindow<Ebook>();
            window.titleContent = new GUIContent("Ebook");
            window.minSize = Vector2.one * 300;
            window.maxSize = Vector2.one * 1000;
        }

        private void Awake()
        {
            ebook.onSuccess = () =>
            {
                ShowNotification(new GUIContent("书籍转换完成！"));
            };
            ebook.onFail = (error) =>
            {
                ShowNotification(new GUIContent("书籍转换失败！" + error));
            };

            value_input = PlayerPrefs.GetString(KEY);

            if (string.IsNullOrEmpty(value_input))
            {
                value_input = Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - 6, 6), "Source/Ebook");
            }
            Redirect();
        }

        private void OnGUI()
        {
            index_view = GUILayout.Toolbar(index_view, text_title);

            GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
            {
                switch (index_view)
                {
                    case 0:
                        RefreshEditorView();
                        break;
                    case 1:
                        RefreshSettingView();
                        break;
                    default:
                        RefreshOtherView();
                        break;
                }
            }
            GUILayout.EndArea();
        }

        private void RefreshEditorView()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("编码格式:", GUILayout.Width(100));

                GUILayout.Label(encoding.ToString());
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("字数限制:", GUILayout.Width(100));

                GUILayout.Label(lineCount.ToString());
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("来源路径:", GUILayout.Width(100));

                GUILayout.Label(InputPath);
            }
            GUILayout.EndHorizontal();

            GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(3));

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(125));
                {
                    scroll_book = GUILayout.BeginScrollView(scroll_book);
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Height(100));
                            {
                                GUILayout.Box(list[i].name, GUILayout.Width(80), GUILayout.ExpandHeight(true));

                                list[i].filter = GUILayout.Toggle(list[i].filter, "");
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("转换", GUILayout.Height(50)))
                    {
                        StartUp();
                    }

                    if (GUILayout.Button("转码"))
                    {
                        Transcoding();
                    }

                    if (GUILayout.Button(select ? "反选" : "全选"))
                    {
                        Select();
                    }

                    if (GUILayout.Button("目录"))
                    {
                        OpenFolder(InputPath);
                    }

                    if (GUILayout.Button("刷新"))
                    {
                        Redirect();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshSettingView()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("编码格式:", GUILayout.Width(100));

                value_code = (CodeType)EditorGUILayout.EnumPopup(value_code);

                if (GUILayout.Button("保存", GUILayout.Width(100)))
                {
                    switch (value_code)
                    {
                        case CodeType.Default:
                            encoding = Encoding.Default;
                            break;
                        case CodeType.ANSI:
                            encoding = Encoding.ASCII;
                            break;
                        case CodeType.Unicode:
                            encoding = Encoding.Unicode;
                            break;
                        case CodeType.UTF8:
                            encoding = Encoding.UTF8;
                            break;
                        case CodeType.UTF32:
                            encoding = Encoding.UTF32;
                            break;
                        case CodeType.Chinese:
                            encoding = Encoding.GetEncoding("GB2312");
                            break;
                        default:
                            encoding = Encoding.Default;
                            break;
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("字数限制:", GUILayout.Width(100));

                value_number = EditorGUILayout.IntField(value_number);

                if (lineCount != value_number)
                {
                    lineCount = value_number;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("来源路径:", GUILayout.Width(100));

                if (GUILayout.Button(InputPath))
                {
                    value_input = EditorUtility.SaveFolderPanel("Intput", InputPath, string.Empty);

                    PlayerPrefs.SetString(KEY, value_input);

                    Redirect();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshOtherView()
        {
            if (GUILayout.Button("联系我们"))
            {
                Application.OpenURL("https://www.baidu.com");
            }
        }

        #region Ebook
        private void StartUp()
        {
            if (Directory.Exists(InputPath))
            {
                foreach (Book book in list)
                {
                    if (book.filter)
                    {
                        ebook.Convert(book.path, encoding);
                    }
                }
                Redirect();
            }
            else
            {
                Directory.CreateDirectory(InputPath);
            }
        }

        private void Redirect()
        {
            list.Clear();

            if (Directory.Exists(InputPath))
            {
                DirectoryInfo root = new DirectoryInfo(InputPath);

                foreach (FileInfo file in root.GetFiles("*.txt", SearchOption.AllDirectories))
                {
                    Book book = new Book()
                    {
                        name = file.Name,

                        path = file.FullName,

                        directory = file.DirectoryName,

                        filter = false,
                    };
                    list.Add(book);
                }
            }
        }

        public void Transcoding()
        {
            if (Directory.Exists(InputPath))
            {
                foreach (Book book in list)
                {
                    string path = string.Format("{0}/{1}_{2}.txt", book.directory, Path.GetFileNameWithoutExtension(book.name), "new");

                    try
                    {
                        if (book.filter)
                        {
                            File.WriteAllText(path, File.ReadAllText(book.path, encoding), Encoding.Default);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                Redirect();
            }
            else
            {
                Debug.LogError("You have to Create a new Floder");
            }
        }

        private string InputPath
        {
            get
            {
                return value_input;
            }
        }
        #endregion

        private void Select()
        {
            select = !select;

            for (int i = 0; i < list.Count; i++)
            {
                list[i].filter = select;
            }
        }

        private void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
            {
                path = path.Replace("/", "\\");

                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            else
            {
                Debug.LogError("No Directory: " + path);
            }
        }

        class Book
        {
            public string name;

            public string path;

            public string directory;

            public bool filter;
        }
    }

    public class EbookConvert
    {
        public Action onSuccess;

        public Action<string> onFail;

        private const string EXTENSION = ".txt";

        private const int MAXNUMBER = 500;

        private readonly List<string> keyword_begin = new List<string> { "(", "[", "{", "<", "（", "【", "《", "‘", "“", "\'", "\"", "「", "第" };

        private readonly List<string> keyword_among = new List<string> { "《", "》", "章", "节", "篇" };

        private readonly List<string> keyword_ending = new List<string> { ".", "。", "!", "！", "?", "？", "\'", "\"", "”", "’", ")", "]", "}", ">", "）", "】", "》", "」", "…" };

        public void Convert(string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source)) return;

            if (!File.Exists(source)) return;

            string document = string.Format("{0}/{1}{2}{3}", Path.GetDirectoryName(source), Path.GetFileNameWithoutExtension(source), DateTime.Now.ToString("HHmm"), EXTENSION);

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