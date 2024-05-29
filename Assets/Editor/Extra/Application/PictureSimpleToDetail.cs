using Game;
using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor.Window
{
    public class PictureSimpleToDetail : CustomWindow
    {
        private readonly string app = "D:/Program Files/realesrgan-ncnn-vulkan-20210901-windows/realesrgan-ncnn-vulkan.exe";

        private Process process;

        [MenuItem("Extra/Picture")]
        protected static void Open()
        {
            Open<PictureSimpleToDetail>("图片转高清");
        }

        protected override void Initialise() { }

        protected override void Refresh()
        {
            if (GUILayout.Button(string.IsNullOrEmpty(input.value) ? "选择图片" : input.value))
            {
                input.value = EditorUtility.OpenFilePanelWithFilters("图片选择", Application.dataPath, new string[] { "Image files", "png,jpg,jpeg" });
            }

            if (GUILayout.Button(ToLanguage("Simple To Detail")))
            {
                if (string.IsNullOrEmpty(input.value)) return;

                string output = Utility.Path.NewFile(input.value);

                string arguments = string.Format("-i {0} -o {1}", input.value, output);

                if (process != null && !process.HasExited)
                {
                    ShowNotification("正在处理中...");
                }
                else
                {
                    process = Process.Start(app, arguments);

                    process.EnableRaisingEvents = true;

                    process.Exited += OnExited;
                }
            }
        }

        private void OnExited(object sender, System.EventArgs e)
        {
            AssetDatabase.Refresh();
        }
    }
}