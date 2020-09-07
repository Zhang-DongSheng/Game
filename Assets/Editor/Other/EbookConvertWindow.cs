using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public class EbookConvertWindow : EditorWindow
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

        private readonly string label_code = "编码格式";

        private readonly string label_input = "来源路径";

        private readonly string label_output = "输出路径";

        private readonly string label_number = "每段字数限制";

        private readonly string label_save = "保存";

        private readonly string label_switch = "转换";

        private readonly string label_refresh = "刷新";

        private readonly string label_openInputFolder = "打开输入文件夹";

        private readonly string label_openOutputFolder = "打开输出文件夹";

        private readonly string label_refreshAsset = "刷新本地资源";

        private readonly EbookConvert ebook = new EbookConvert();

        #region Temporary Variable
        private int index_view;

        private Vector2 scroll_book;

        private CodeType value_code;

        private Rect rect_input;

        private string value_input;

        private Rect rect_output;

        private string value_output;

        private int value_number = -1;
        #endregion

        #region Param
        private Encoding encoding = Encoding.Default;

        private string inputPath = "Source/Ebook/Input";

        private string outputPath = "Source/Ebook/Output";

        private int lineCount = 1000;

        private readonly List<Book> list = new List<Book>();
        #endregion

        [MenuItem("Other/Ebook")]
        private static void Open()
        {
            EditorWindow window = EditorWindow.GetWindow<EbookConvertWindow>();
            window.minSize = Vector2.one * 300;
            window.maxSize = Vector2.one * 1000;
            window.titleContent = new GUIContent("Ebook");
        }

        private void Awake()
        {
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
                GUILayout.Label(label_code, GUILayout.Width(100));

                GUILayout.Label(encoding.ToString());
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label_input, GUILayout.Width(100));

                GUILayout.Label(InputPath);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label_output, GUILayout.Width(100));

                GUILayout.Label(OutputPath);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label_number, GUILayout.Width(100));

                GUILayout.Label(lineCount.ToString());
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

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

                GUILayout.Space(15);

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button(label_refresh))
                    {
                        Redirect();
                    }

                    if (GUILayout.Button(label_switch))
                    {
                        StartUp();
                    }

                    if (GUILayout.Button(label_openInputFolder))
                    {
                        OpenFolder(InputPath);
                    }

                    if (GUILayout.Button(label_openOutputFolder))
                    {
                        OpenFolder(OutputPath);
                    }

                    if (GUILayout.Button(label_refreshAsset))
                    {
                        AssetDatabase.Refresh();
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
                GUILayout.Label(label_code, GUILayout.Width(100));

                value_code = (CodeType)EditorGUILayout.EnumPopup(value_code);

                if (GUILayout.Button(label_save, GUILayout.Width(100)))
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
                GUILayout.Label(label_input, GUILayout.Width(100));

                rect_input = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 247));

                value_input = EditorGUI.TextField(rect_input, value_input);

                if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect_input.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        value_input = DragAndDrop.paths[0].Remove(0, 7);
                    }
                }

                if (GUILayout.Button(label_save, GUILayout.Width(100)))
                {
                    inputPath = value_input;

                    Redirect();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label_output, GUILayout.Width(100));

                rect_output = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 247));

                value_output = EditorGUI.TextField(rect_output, value_output);

                if ((Event.current.type == EventType.DragUpdated ||
                    Event.current.type == EventType.DragExited) &&
                rect_output.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        value_output = DragAndDrop.paths[0].Remove(0, 7);
                    }
                }

                if (GUILayout.Button(label_save, GUILayout.Width(100)))
                {
                    outputPath = value_output;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label_number, GUILayout.Width(100));

                value_number = EditorGUILayout.IntField(value_number);

                if (GUILayout.Button(label_save, GUILayout.Width(100)))
                {
                    lineCount = value_number;
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
                DirectoryInfo root = new DirectoryInfo(InputPath);

                int index = 0;

                foreach (FileInfo file in root.GetFiles())
                {
                    if (file.Extension.Equals(".txt"))
                    {
                        if (Filter(index++, file.Name))
                        {
                            ebook.Convert(file.FullName, encoding);
                        }
                    }
                }
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

                foreach (FileInfo file in root.GetFiles())
                {
                    if (file.Extension.Equals(".txt"))
                    {
                        Book book = new Book()
                        {
                            name = file.Name,
                            filter = false,
                        };
                        list.Add(book);
                    }
                }
            }
        }

        private bool Filter(int index, string name)
        {
            bool result = false;

            if (list.Count > 0)
            {
                Book book = list.Find(x => x.name.Equals(name));

                result = book != null ? book.filter : true;
            }
            else
            {
                result = true;
            }

            return result;
        }

        private string InputPath
        {
            get
            {
                return Path.Combine(Application.dataPath, inputPath);
            }
        }

        private string OutputPath
        {
            get
            {
                return Path.Combine(Application.dataPath, outputPath);
            }
        }
        #endregion

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

            public bool filter;
        }
    }

    public class EbookConvert
    {
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
                Debug.LogError(e.Message);
            }
            finally
            {

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