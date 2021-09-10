using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    public class ResourceSynchronization : CustomWindow
    {
        [MenuItem("Data/Synchronization")]
        protected static void Open()
        {
            Open<ResourceSynchronization>("资源同步");
        }

        protected override void Init()
        {

        }

        protected override void Refresh()
        {
            if (GUILayout.Button("同步ILRuntime"))
            {
                SynchronizationILRuntime();
            }
        }

        private void SynchronizationILRuntime()
        {
            string key = "Hotfix";

            string src = string.Format("{0}/ILRuntime/Hotfix~/bin/Debug/{1}.dll", Application.dataPath, key);

            string dst = string.Format("{0}/HotFix/{1}.dll", Application.streamingAssetsPath, key);

            Replace(src, dst);

            src = string.Format("{0}/ILRuntime/Hotfix~/bin/Debug/{1}.pdb", Application.dataPath, key);

            dst = string.Format("{0}/HotFix/{1}.pdb", Application.streamingAssetsPath, key);

            Replace(src, dst);

            ShowNotification("同步完成！");
        }

        private void Replace(string src, string dst)
        {
            if (File.Exists(dst))
            {
                File.Delete(dst);
            }
            File.Copy(src, dst);
        }
    }
}