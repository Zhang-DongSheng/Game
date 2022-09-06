using Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityEditor.Window
{
    public class BuildPackage : CustomWindow
    {
        private readonly Input inputCompany = new Input();

        private readonly Input inputProduct = new Input();

        private readonly Input inputVersion = new Input();

        [MenuItem("File/Build")]
        protected static void Open()
        {
            Open<BuildPackage>("一键打包");
        }

        protected override void Init()
        {
            inputCompany.value = PlayerSettings.companyName;

            inputProduct.value = PlayerSettings.productName;

            inputVersion.value = PlayerSettings.bundleVersion;
        }

        protected override void Refresh()
        {
            inputCompany.value = GUILayout.TextField(inputCompany.value);

            inputProduct.value = GUILayout.TextField(inputProduct.value);

            inputVersion.value = GUILayout.TextField(inputVersion.value);

            if (GUILayout.Button(""))
            {
                Package();
            }
        }

        private void Package()
        {
            PlayerSettings.companyName = inputCompany.value;

            PlayerSettings.productName = inputProduct.value;

            PlayerSettings.bundleVersion = inputVersion.value;

            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel26;

            BuildPlayerOptions options = new BuildPlayerOptions();

            int count = EditorBuildSettings.scenes.Length;

            string[] scenes = new string[count];

            for (int i = 0; i < count; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            options.scenes = scenes;

            string path = Utility.Path.Project;

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    options.target = BuildTarget.Android;
                    options.locationPathName = string.Format("{0}/{1}/{2}_{3}.apk", path, options.target, PlayerSettings.productName, PlayerSettings.bundleVersion);
                    break;
                case BuildTarget.iOS:
                    options.target = BuildTarget.iOS;
                    options.locationPathName = string.Format("{0}/{1}/{2}_{3}.ipa", path, options.target, PlayerSettings.productName, PlayerSettings.bundleVersion);
                    break;
                default:
                    options.target = BuildTarget.StandaloneWindows;
                    options.locationPathName = string.Format("{0}/{1}/{2}_{3}.exe", path, options.target, PlayerSettings.productName, PlayerSettings.bundleVersion);
                    break;
            }
            options.options = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(options);

            BuildSummary summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Success(summary);
                    break;
                case BuildResult.Failed:
                    Fail(summary);
                    break;
                default:
                    Debug.LogWarning("Unknow Exception!");
                    break;
            }
        }

        private void Success(BuildSummary summary)
        {
            string size = "检测打包大小失败";

            //if (File.Exists())
            //{
            //    FileInfo apk = new FileInfo(androidPath);
            //    size = " " + (apk.Length / (1024.00 * 1024.00)).ToString("f2") + "MB";
            //}

            //string time = " " + summary.totalTime + "s";

            //buildResult = "打包成功: " + summary.outputPath + "\n" +
            //    "安装后大小: " + size + "\n" +
            //    "打包时长: " + time + "\n";

            //EditorUtility.OpenWithDefaultApp(buildPath.Replace(@"/", @"\"));
        }

        private void Fail(BuildSummary summary)
        {
            ShowNotification(summary.result.ToString());
        }
    }
}