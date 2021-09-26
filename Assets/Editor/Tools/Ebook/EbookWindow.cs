using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Ebook;
using UnityEngine;

namespace UnityEditor.Window
{
    class EbookWindow : CustomWindow
    {
        private readonly string[] text_title = new string[4] { "主页", "编辑", "下载", "其他" };

        private readonly Encoding encoding = new UTF8Encoding(false);

        private readonly TextComposition composition = new TextComposition();

        private readonly ConvertFormat convert = new ConvertFormat();

        private readonly Downloader downloader = new Downloader();

        private readonly List<Book> books = new List<Book>();

        private readonly Index number = new Index();

        private bool select = false;

        [MenuItem("Application/Ebook")]
        protected static void Open()
        {
            Open<EbookWindow>("电子书");
        }

        protected override void Init()
        {
            composition.onSuccess = () =>
            {
                ShowNotification("书籍转换完成！");
            };
            composition.onFail = (error) =>
            {
                ShowNotification("书籍转换失败！" + error);
            };

            input.action = (value) =>
            {
                EbookConfig.Path = value; Redirect();
            };
            input.value = EbookConfig.Path;

            number.action = (value) =>
            {
                EbookConfig.Number = value;
            };
            number.index = EbookConfig.Number;
        }

        protected override void Refresh()
        {
            index.index = GUILayout.Toolbar(index.index, text_title);

            GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
            {
                switch (index.index)
                {
                    case 0:
                        RefreshMain();
                        break;
                    case 1:
                        RefreshEditor();
                        break;
                    case 2:
                        RefreshDownload();
                        break;
                    default:
                        RefreshSetting();
                        break;
                }
            }
            GUILayout.EndArea();
        }

        private void RefreshMain()
        {

        }

        private void RefreshEditor()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("路径:", GUILayout.Width(50));

                if (GUILayout.Button(input.value))
                {
                    string path = EditorUtility.SaveFolderPanel("选择路径", input.value, string.Empty);

                    if (!string.IsNullOrEmpty(path))
                    {
                        input.value = path;
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("字数:", GUILayout.Width(50));

                number.index = EditorGUILayout.IntField(number.index);
            }
            GUILayout.EndHorizontal();

            GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(3));

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(125));
                {
                    scroll = GUILayout.BeginScrollView(scroll);
                    {
                        for (int i = 0; i < books.Count; i++)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Height(100));
                            {
                                GUILayout.Box(books[i].name, GUILayout.Width(80), GUILayout.ExpandHeight(true));

                                books[i].filter = GUILayout.Toggle(books[i].filter, "");
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
                        Composition();
                    }

                    if (GUILayout.Button("转码"))
                    {
                        Convert();
                    }

                    if (GUILayout.Button(select ? "反选" : "全选"))
                    {
                        Select();
                    }

                    if (GUILayout.Button("目录"))
                    {
                        OpenFolder(input.value);
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

        private void RefreshDownload()
        {
            if (GUILayout.Button("下载"))
            {
                downloader.Download("https://www.baidu.com");
            }
        }

        private void RefreshSetting()
        {

        }

        private void Composition()
        {
            if (Directory.Exists(input.value))
            {
                foreach (Book book in books)
                {
                    if (book.filter)
                    {
                        composition.Convert(book.path, encoding);
                    }
                }
                Redirect();
            }
            else
            {
                Directory.CreateDirectory(input.value);
            }
        }

        public void Convert()
        {
            if (Directory.Exists(input.value))
            {
                foreach (Book book in books)
                {
                    if (book.filter)
                    {
                        convert.Format(book.path, encoding);
                    }
                }
                Redirect();
            }
            else
            {
                Debug.LogError("You have to Create a new Floder");
            }
        }

        private void Redirect()
        {
            books.Clear();

            if (string.IsNullOrEmpty(input.value)) return;

            if (Directory.Exists(input.value))
            {
                DirectoryInfo root = new DirectoryInfo(input.value);

                foreach (FileInfo file in root.GetFiles("*.txt", SearchOption.AllDirectories))
                {
                    Book book = new Book()
                    {
                        name = file.Name,

                        path = file.FullName,

                        directory = file.DirectoryName,

                        filter = false,
                    };
                    books.Add(book);
                }
            }
        }

        private void Select()
        {
            select = !select;

            for (int i = 0; i < books.Count; i++)
            {
                books[i].filter = select;
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
}