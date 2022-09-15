using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Game;
using UnityEditor.Window;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.Window
{
    public class Game : CustomWindow
    {
        private const string SCRIPT = "Scripts/UI/Panel/UI";

        private const string PREFAB = "Package/Prefab/UI/Panel/UI";

        private string content;

        [MenuItem("Game/UI")]
        protected static void Open()
        {
            Open<Game>("UIÄ£°å±à¼­");
        }

        protected override void Init()
        {

        }

        protected override void Refresh()
        {
            this.content = GUILayout.TextField(content);

            if (GUILayout.Button("Éú³É"))
            {
                if (string.IsNullOrEmpty(content))
                {
                    return;
                }
                string path = string.Format("Assets/{0}{1}.cs", SCRIPT, content);

                ScriptHelper.Create(path);

                AssetDatabase.Refresh();

                path = string.Format("Assets/{0}{1}.prefab", PREFAB, content);

                GameObject prefab = PrefabHelper.Create(path);

                AssetDatabase.Refresh();


                string typeName = string.Format("Game.UI.UI{0}", content);

                Debug.Log(Assembly.GetExecutingAssembly().FullName);

                Assembly assembly = Assembly.Load("Assembly-CSharp");

                Type type = assembly.GetType(typeName);

                Debug.LogError(type.Name);

                prefab.AddComponent(type);

                PrefabUtility.SavePrefabAsset(prefab);

            }
        }
    }
}