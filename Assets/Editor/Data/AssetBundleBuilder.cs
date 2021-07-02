using Game;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    class AssetBundleBuilder : EditorWindow
    {
        private readonly List<string> IgnoreExtensionList = new List<string>() { ".meta", ".manifest" };

        #region GUI
        private readonly string[] text_view = new string[] { "资源", "Bundle", "其他" };

        private int index_view;

        private int index_source;

        private int index_assetbundle;

        private string input_source;

        private string input_assetbundle;

        private Vector2 scroll;
        #endregion

        private int currentViewIndex = -1;

        private int currentSourceIndex = -1;

        private int currentAssetbundleIndex = -1;

        private string[] text_folder_source = new string[] { "None" };

        private string[] text_folder_assetbundle = new string[] { "None" };

        private readonly List<ItemFolder> folder_source = new List<ItemFolder>();

        private readonly List<ItemFolder> folder_assetbundle = new List<ItemFolder>();

        private readonly List<ItemFile> items = new List<ItemFile>();

        [MenuItem("Data/AssetBundle")]
        protected static void Open()  
        {
            AssetBundleBuilder window = EditorWindow.GetWindow<AssetBundleBuilder>();
            window.titleContent = new GUIContent("Asset");
            window.minSize = new Vector2(500, 300);
            window.Init();
            window.Show();
        }

        private void Init()
        {
            UpdateAssetFolder();

            UpdateAssetBundleFolder();
        }

        private void UpdateAssetFolder()
        {
            string path = SourcePath;

            folder_source.Clear();

            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                text_folder_source = new string[1 + root.GetDirectories().Length];

                int index = 0;

                folder_source.Add(new ItemFolder()
                {
                    name = root.Name,
                    path = root.FullName,
                });
                text_folder_source[index++] = root.Name;

                foreach (DirectoryInfo folder in root.GetDirectories())
                {
                    folder_source.Add(new ItemFolder()
                    {
                        name = string.Format("{0}/{1}", root.Name, folder.Name),
                        path = folder.FullName,
                    });
                    text_folder_source[index++] = folder.Name;
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        private void UpdateAssetBundleFolder()
        {
            string path = AssetBundlePath + "/" + GameConfig.ArtPath;

            folder_assetbundle.Clear();

            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                text_folder_assetbundle = new string[1 + root.GetDirectories().Length];

                int index = 0;

                folder_assetbundle.Add(new ItemFolder()
                {
                    name = root.Name,
                    path = root.FullName,
                });
                text_folder_assetbundle[index++] = root.Name;

                foreach (DirectoryInfo folder in root.GetDirectories())
                {
                    folder_assetbundle.Add(new ItemFolder()
                    {
                        name = string.Format("{0}/{1}", root.Name, folder.Name),
                        path = folder.FullName,
                    });
                    text_folder_assetbundle[index++] = folder.Name;
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        private void OnGUI()
        {
            index_view = GUILayout.Toolbar(index_view, text_view);

            if (currentViewIndex != index_view)
            {
                currentSourceIndex = currentAssetbundleIndex = -1;

                currentViewIndex = index_view;
            }

            GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
            {
                switch (currentViewIndex)
                {
                    case 0:
                        RefreshUIAsset();
                        break;
                    case 1:
                        RefreshUIAssetBundle();
                        break;
                    default:
                        RefreshUIOther();
                        break;
                }
            }
            GUILayout.EndArea();
        }

        private void RefreshUIAsset()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Asset:", GUILayout.Width(50));

                index_source = EditorGUILayout.Popup(index_source, text_folder_source);

                if (currentSourceIndex != index_source)
                {
                    currentSourceIndex = index_source;

                    if (folder_source.Count > currentSourceIndex)
                    {
                        if (currentSourceIndex == 0)
                            LoadAll(folder_source[0].path, folder_source[0].name, input_source, true);
                        else
                            Load(folder_source[currentSourceIndex].path, folder_source[currentSourceIndex].name, true);
                    }
                    else
                    {
                        items.Clear();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (currentSourceIndex == 0)
                {
                    GUILayout.Label("Search:", GUILayout.Width(50));

                    input_source = GUILayout.TextField(input_source);

                    if (GUILayout.Button("Refresh", GUILayout.Width(100)))
                    {
                        if (folder_source.Count > 0)
                        {
                            LoadAll(folder_source[0].path, folder_source[0].name, input_source);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (items.Count > 0)
                {
                    RefreshUIList();

                    GUILayout.Space(5);

                    GUILayout.BeginVertical(GUILayout.Width(100));
                    {
                        if (GUILayout.Button("生成[AssetBundle]", GUILayout.Height(30)))
                        {
                            BuildAssetBundle();
                        }

                        if (GUILayout.Button("全选"))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = true;
                            }
                        }

                        if (GUILayout.Button("反选"))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = false;
                            }
                        }

                        if (GUILayout.Button("资源文件夹"))
                        {
                            OpenFolder(SourcePath);
                        }

                        if (GUILayout.Button("Bundle文件夹"))
                        {
                            OpenFolder(AssetBundlePath);
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshUIAssetBundle()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Asset:", GUILayout.Width(50));

                index_assetbundle = EditorGUILayout.Popup(index_assetbundle, text_folder_assetbundle);

                if (currentAssetbundleIndex != index_assetbundle)
                {
                    currentAssetbundleIndex = index_assetbundle;

                    if (folder_assetbundle.Count > currentAssetbundleIndex)
                    {
                        if (currentAssetbundleIndex == 0)
                            LoadAll(folder_assetbundle[0].path, folder_assetbundle[0].name, input_assetbundle);
                        else
                            Load(folder_assetbundle[currentAssetbundleIndex].path, folder_assetbundle[currentAssetbundleIndex].name);
                    }
                    else
                    {
                        items.Clear();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (currentAssetbundleIndex == 0)
                {
                    GUILayout.Label("Search:", GUILayout.Width(50));

                    input_assetbundle = GUILayout.TextField(input_assetbundle);

                    if (GUILayout.Button("Refresh", GUILayout.Width(100)))
                    {
                        if (folder_assetbundle.Count > 0)
                        {
                            LoadAll(folder_assetbundle[0].path, folder_assetbundle[0].name, input_assetbundle);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (items.Count > 0)
                {
                    RefreshUIList();

                    GUILayout.Space(5);

                    GUILayout.BeginVertical(GUILayout.Width(100));
                    {
                        if (GUILayout.Button("上传", GUILayout.Height(60)))
                        {
                            Upload();

                            MD5Tools.Record(HistoryPath, string.Empty, items);
                        }

                        if (GUILayout.Button("全选"))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = true;
                            }
                        }

                        if (GUILayout.Button("反选"))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = false;
                            }
                        }

                        if (GUILayout.Button("打开MD5"))
                        {
                            OpenFileMd5();
                        }

                        if (GUILayout.Button("更新MD5"))
                        {
                            UploadMd5();
                        }

                        if (GUILayout.Button("删除MD5"))
                        {
                            string path = HistoryPath;

                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshUIList()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("S", GUILayout.Width(20));
                    GUILayout.Label("名称", GUILayout.Width(120));
                    GUILayout.Label("路径");
                    GUILayout.Label("大小", GUILayout.Width(100));
                }
                GUILayout.EndHorizontal();

                scroll = GUILayout.BeginScrollView(scroll);
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            items[i].select = GUILayout.Toggle(items[i].select, string.Empty, GUILayout.Width(20));
                            GUILayout.Label(items[i].name, GUILayout.Width(120));
                            GUILayout.Label(items[i].path);
                            GUILayout.Label(items[i].length.ToSize(), GUILayout.Width(100));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void RefreshUIOther()
        {
            if (GUILayout.Button("联系我们"))
            {
                Application.OpenURL("https://www.baidu.com");
            }
        }

        private void Load(string path, string folder = null, bool asset = false)
        {
            if (string.IsNullOrEmpty(path)) return;

            items.Clear();

            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                foreach (FileInfo file in root.GetFiles())
                {
                    if (!IgnoreExtensionList.Contains(file.Extension))
                    {
                        if (!string.IsNullOrEmpty(folder) && folder.EndsWith(file.Name)) { }
                        else
                        {
                            items.Add(new ItemFile()
                            {
                                name = file.Name,
                                path = asset ? AssetPath(file.FullName) : file.FullName,
                                root = folder,
                                length = file.Length,
                                select = true,
                            });
                        }
                    }
                }
            }
        }

        private void LoadAll(string path, string folder = null, string predicate = null, bool asset = false)
        {
            if (string.IsNullOrEmpty(path)) return;

            items.Clear();

            if (Directory.Exists(path))
            {
                List<ItemFile> files = new List<ItemFile>();

                Find(path, folder, asset, ref files);

                if (string.IsNullOrEmpty(predicate))
                {
                    items.AddRange(files);
                }
                else
                {
                    items.AddRange(files.FindAll(x => x.name.Contains(predicate)));
                }
            }
        }

        private void Find(string path, string type, bool asset, ref List<ItemFile> files)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                foreach (FileInfo file in root.GetFiles())
                {
                    if (!IgnoreExtensionList.Contains(file.Extension))
                    {
                        if (!string.IsNullOrEmpty(type) && type.EndsWith(file.Name)){ }
                        else
                        {
                            files.Add(new ItemFile()
                            {
                                name = file.Name,
                                path = asset ? AssetPath(file.FullName) : file.FullName,
                                root = type,
                                length = file.Length,
                                select = true,
                            });
                        }
                    }
                }

                foreach (DirectoryInfo dir in root.GetDirectories())
                {
                    Find(dir.FullName, string.Format("{0}/{1}", type, dir.Name), asset, ref files);
                }
            }
        }

        private void BuildAssetBundle()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].select)
                {
                    BuildAssetBundle(items[i]);
                }
            }

            UpdateAssetBundleFolder();

            ShowNotification(new GUIContent("Build AssetBundle Success!"));
        }

        private void BuildAssetBundle(ItemFile file)
        {
            string path = string.Format("{0}/{1}", AssetBundlePath, file.root);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

            AssetBundleBuild build = new AssetBundleBuild()
            {
                assetBundleName = Path.GetFileNameWithoutExtension(file.name),
                assetNames = new string[] { file.path }
            };
            builds.Add(build);

            BuildPipeline.BuildAssetBundles(path, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
        }

        private void Upload()
        {
            ShowNotification(new GUIContent("Upload Done!"));
        }

        private void UploadMd5()
        {
            string path = HistoryPath;

            if (File.Exists(path))
            {
                MD5Tools.Recompilation(path);
            }
            else
            { 
                
            }
        }

        private void OpenFileMd5()
        {
            string path = HistoryPath;

            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start("notepad.exe", path);
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

        private string AssetPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            path = path.Replace("\\", "/");

            if (path.Contains("Assets/"))
            {
                int index = path.LastIndexOf("Assets/");

                path = path.Remove(0, index);
            }
            return path;
        }

        private string SourcePath
        {
            get
            {
                return Application.dataPath + "/" + GameConfig.ArtPath;
            }
        }

        private string AssetBundlePath
        {
            get
            {
                return string.Format("{0}{1}/{2}", Application.dataPath.Remove(Application.dataPath.Length - 6, 6), GameConfig.AssetBundlePath, GameConfig.BuildTarget);
            }
        }

        private string HistoryPath
        {
            get
            {
                return AssetBundlePath + "/" + GameConfig.History;
            }
        }

        #region Select
        [MenuItem("Assets/Build AssetBundle")]
        protected static void BuildAssetToAssetBundle()
        {
            string folder = Application.dataPath.Remove(Application.dataPath.Length - 6) + "AssetBundle/Select/";

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            if (Selection.objects != null && Selection.objects.Length > 0)
            {
                List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    AssetBundleBuild build = new AssetBundleBuild()
                    {
                        assetBundleName = Selection.objects[i].name,
                        assetNames = new string[] { AssetDatabase.GetAssetPath(Selection.objects[i]) }
                    };
                    builds.Add(build);
                }
                BuildPipeline.BuildAssetBundles(folder, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
            }
        }
        #endregion
    }
}