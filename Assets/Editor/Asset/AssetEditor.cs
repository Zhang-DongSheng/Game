using System.IO;
using System.Text;
using UnityEngine;
using Utils;

namespace UnityEditor
{
    public class AssetEditor
    {
        [MenuItem("Assets/Convert Encoding", false, 24)]
        protected static void ConvertEncoding()
        {
            Object asset = Selection.activeObject;

            if (asset != null && asset is TextAsset script)
            {
                string path = AssetDatabase.GetAssetPath(asset);

                Encoding encoding = FileEncoding.Get(path);

                Encoding UTF8 = new UTF8Encoding(false);

                if (encoding != UTF8)
                {
                    File.WriteAllText(path, File.ReadAllText(path, encoding), UTF8);
                }
                AssetDatabase.Refresh(); ShowNotification("转换成功！");
            }
            else
            {
                ShowNotification("选中目标不可转换！");
            }
        }

        protected static void ShowNotification(string message)
        {
            EditorWindow.focusedWindow.ShowNotification(new GUIContent(message));
        }
    }
}