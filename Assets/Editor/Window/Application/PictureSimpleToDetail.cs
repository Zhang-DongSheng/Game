using Game.Utils;
using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor.Window
{
    public class PictureSimpleToDetail : CustomWindow
    {
        private readonly string app = "D:/Program Files/realesrgan-ncnn-vulkan-20210901-windows/realesrgan-ncnn-vulkan.exe";

        private Process process;

        [MenuItem("Application/Picture")]
        protected static void Open()
        {
            Open<PictureSimpleToDetail>("ͼƬת����");
        }

        protected override void Init() { }

        protected override void Refresh()
        {
            if (GUILayout.Button(string.IsNullOrEmpty(input.value) ? "ѡ��ͼƬ" : input.value))
            {
                input.value = EditorUtility.OpenFilePanelWithFilters("ͼƬѡ��", Application.dataPath, new string[] { "Image files", "png,jpg,jpeg" });
            }

            if (GUILayout.Button("Simple To Detail"))
            {
                if (string.IsNullOrEmpty(input.value)) return;

                string output = FileUtils.New(input.value);

                string arguments = string.Format("-i {0} -o {1}", input.value, output);

                if (process != null && !process.HasExited)
                {
                    ShowNotification("���ڴ�����...");
                }
                else
                {
                    process = Process.Start(app, arguments);

                    process.EnableRaisingEvents = true;

                    process.Exited += OnExited;
                }
            }
        }

        private void OnExited(object sender, EventArgs e)
        {
            AssetDatabase.Refresh();
        }
    }
}