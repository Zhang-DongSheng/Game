using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace UnityEditor
{
    public class AssetBundleBuilder : EditorWindow
    {
        private const string SOURCE = "Art";

        private const string ASSETBUNDLE = "AssetBundle";

        private const string HISTORY = "history.txt";

        private readonly List<string> IgnoreExtensionList = new List<string>() { ".meta", ".manifest" };

        #region GUI
        private readonly string[] text_view = new string[] { "AssetBundle", "Upload", "Other" };

        private int index_view;

        private int index_source;

        private int index_assetbundle;

        private Vector2 scroll;
        #endregion

        private int currentViewIndex;

        private int currentSourceIndex;

        private int currentAssetbundleIndex;

        private string[] text_folder_source = new string[] { "None" };

        private string[] text_folder_assetbundle = new string[] { "None" };

        private readonly List<ItemFolder> folder_source = new List<ItemFolder>();

        private readonly List<ItemFolder> folder_assetbundle = new List<ItemFolder>();

        private readonly List<ItemFile> items = new List<ItemFile>();

        private readonly List<ItemUpload> m_asset = new List<ItemUpload>();

        [MenuItem("Source/Upload")]
        private static void Open()
        {
            AssetBundleBuilder window = EditorWindow.GetWindow<AssetBundleBuilder>();
            window.titleContent = new GUIContent("Asset");
            window.minSize = new Vector2(500, 300);
            window.Init();
            window.Show();
        }

        private void Init()
        {
            //QCloudCosSdk.Init();

            InitFolder();
        }

        private void InitFolder()
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
                        name = root.Name + "/" + folder.Name,
                        path = folder.FullName,
                    });
                    text_folder_source[index++] = folder.Name;
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            path = AssetBundlePath + "/" + SOURCE;

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
                        name = root.Name + "/" + folder.Name,
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
                        RefreshUIAssetBundle();
                        break;
                    case 1:
                        RefreshUIUpload();
                        break;
                    default:
                        RefreshUIOther();
                        break;
                }
            }
            GUILayout.EndArea();
        }

        private void RefreshUIAssetBundle()
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
                        LoadFile(folder_source[currentSourceIndex].path, folder_source[currentSourceIndex].name, true);
                    }
                    else
                    {
                        items.Clear();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (items.Count > 0)
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("S", GUILayout.Width(20));
                            GUILayout.Label("Name", GUILayout.Width(120));
                            GUILayout.Label("Path");
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
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();

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

                        if (GUILayout.Button("打开文件夹：Source"))
                        {
                            OpenFolder(SourcePath);
                        }

                        if (GUILayout.Button("打开文件夹：AssetBundle"))
                        {
                            OpenFolder(AssetBundlePath);
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshUIUpload()
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
                        LoadFile(folder_assetbundle[currentAssetbundleIndex].path, folder_assetbundle[currentAssetbundleIndex].name);
                    }
                    else
                    {
                        items.Clear();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (items.Count > 0)
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("S", GUILayout.Width(20));
                            GUILayout.Label("Name", GUILayout.Width(120));
                            GUILayout.Label("Path");
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
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();

                    GUILayout.Space(5);

                    GUILayout.BeginVertical(GUILayout.Width(100));
                    {
                        if (GUILayout.Button("上传", GUILayout.Height(60)))
                        {
                            //Upload();

                            RecordHistory();
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
                            OpenMD5();
                        }

                        if (GUILayout.Button("更新MD5"))
                        {
                            UploadMD5();
                        }

                        if (GUILayout.Button("删除MD5"))
                        {
                            DeleteMD5();
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshUIOther()
        {
            if (GUILayout.Button("联系我们"))
            {
                Application.OpenURL("https://www.baidu.com");
            }
        }

        private void LoadFile(string path, string type = null, bool asset = false)
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
                        items.Add(new ItemFile()
                        {
                            name = file.Name,
                            path = asset ? AssetPath(file.FullName) : file.FullName,
                            type = type,
                            select = true,
                        });
                    }
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
            ShowNotification(new GUIContent("Build AssetBundle Success!"));
        }

        private void BuildAssetBundle(ItemFile file)
        {
            string path = string.Format("{0}/{1}", AssetBundlePath, file.type);

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
            int count = m_asset.Count;

            for (int i = 0; i < count; i++)
            {
                int index = i + 1;

                string name = m_asset[i].name;

                //QCloudCosSdk.PutLocalFile( m_asset[i].path, m_asset[i].url, delegate ()
                //{
                //    EditorUtility.DisplayProgressBar(string.Format("资源上传中({0}/{1})", index, count), "update...", index / count);
                //}, 
                //delegate ()
                //{
                //    Debug.LogError("资源上传失败: " + name);
                //});
            }
            EditorUtility.ClearProgressBar();

            ShowNotification(new GUIContent("Upload Done!"));
        }

        private void UploadMD5()
        {
            string path = HistoryPath;

            string url = "information/md5file.txt";

            if (!File.Exists(path)) return;

            Dictionary<string, string> md5 = new Dictionary<string, string>();

            List<string> lines = new List<string>();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);

                string line = reader.ReadLine();

                string[] param = new string[2];

                while (!string.IsNullOrEmpty(line))
                {
                    param = line.Split('|');

                    if (param.Length == 2)
                    {
                        if (md5.ContainsKey(param[0]))
                        {
                            md5[param[0]] = param[1];
                        }
                        else
                        {
                            md5.Add(param[0], param[1]);
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            foreach (var line in md5)
            {
                lines.Add(string.Join("|", line.Key, line.Value));
            }
            File.WriteAllLines(path, lines);

            //QCloudCosSdk.PutLocalFile(path, url, delegate ()
            //{
            //    ShowNotification(new GUIContent("MD5 资源上传成功！"));
            //},
            //delegate ()
            //{
            //    Debug.LogError("资源上传失败: " + name);
            //});
        }

        private void OpenMD5()
        {
            string path = HistoryPath;

            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start("notepad.exe", path);
            }
        }

        private void DeleteMD5()
        {
            string path = HistoryPath;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private string ComputeMD5(string path)
        {
            string result = string.Empty;

            try
            {
                if (File.Exists(path))
                {
                    byte[] buffer = File.ReadAllBytes(path);

                    MD5 md5 = new MD5CryptoServiceProvider();

                    byte[] hash = md5.ComputeHash(buffer);

                    foreach (byte v in hash)
                    {
                        result += Convert.ToString(v, 16);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return result;
        }

        private void RecordHistory()
        {
            string path = HistoryPath;

            string key, value;

            if (File.Exists(path))
            {
                StreamWriter writer = new StreamWriter(path, true);

                for (int i = 0; i < items.Count; i++)
                {
                    key = items[i].type + "/" + items[i].name;

                    value = ComputeMD5(items[i].path);

                    writer.WriteLine(string.Format("{0}|{1}", key, value));
                }

                writer.Dispose();
            }
            else
            {
                FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

                StreamWriter writer = new StreamWriter(stream);

                for (int i = 0; i < items.Count; i++)
                {
                    key = items[i].name;

                    value = ComputeMD5(items[i].path);

                    writer.WriteLine(string.Format("{0}|{1}", key, value));
                }

                writer.Dispose();

                stream.Dispose();
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
                return Application.dataPath + "/" + SOURCE;
            }
        }

        private string AssetBundlePath
        {
            get
            {
                return Application.dataPath.Remove(Application.dataPath.Length - 6, 6) + ASSETBUNDLE;
            }
        }

        private string HistoryPath
        {
            get
            {
                return AssetBundlePath + "/" + HISTORY;
            }
        }

        class ItemUpload
        {
            public string name;

            public string path;

            public string url;
        }

        #region Select
        [MenuItem("Assets/Build AssetBundle")]
        private static void BuildAssetToAssetBundle()
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