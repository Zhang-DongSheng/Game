using System;
using System.Reflection;
using UnityEditor.AssetImporters;
using UnityEditor.Game;
using UnityEngine;

namespace UnityEditor.Window
{
    public class Game : CustomWindow
    {
        private const string SCRIPT = "Scripts/UI/Panel/";

        private const string PREFAB = "Package/Prefab/UI/Panel/";

        private string content;

        [MenuItem("Game/UI")]
        protected static void Open()
        {
            Open<Game>("UIģ��༭");
        }

        protected override void Init()
        {

        }

        protected override void Refresh()
        {
            this.content = GUILayout.TextField(content);

            if (GUILayout.Button("����"))
            {
                if (string.IsNullOrEmpty(content)) return;

                string path = string.Format("Assets/{0}{1}.cs", SCRIPT, content);

                ScriptHelper.Create(path);

                AssetDatabase.Refresh();

                path = string.Format("Assets/{0}{1}.prefab", PREFAB, content);

                PrefabHelper.CreateUGUI(path);

                AssetDatabase.Refresh();

                ShowNotification("ģ�崴�����");

                // �����������ҳ��ö��
            }
        }
    }
}