using Game;
using UnityEngine;

namespace UnityEditor.Window
{
    public class ArtistMd5 : ArtistBase
    {
        private readonly Input inputString = new Input();

        private readonly Input inputFile = new Input();

        private string message;

        public override string Name => "Md5";

        public override void Initialise() { }

        public override void Refresh()
        {
            GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
            {
                GUILayout.BeginHorizontal(GUILayout.Height(25));
                {
                    GUILayout.Label("String", GUILayout.Width(50));

                    inputString.value = GUILayout.TextField(inputString.value);

                    if (GUILayout.Button("Confirm", GUILayout.Width(60)))
                    {
                        if (string.IsNullOrEmpty(inputString.value))
                        {
                            ShowNotification("Content cannot be empty!");
                        }
                        else
                        {
                            message = Utility.Md5.ComputeContent(inputString.value);
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(25));
                {
                    GUILayout.Label("File", GUILayout.Width(50));

                    if (GUILayout.Button(inputFile.value))
                    {
                        inputFile.value = EditorUtility.OpenFilePanel("Md5", "", "");
                    }
                    // Compute File Md5
                    if (GUILayout.Button("Confirm", GUILayout.Width(60)))
                    {
                        message = Utility.Md5.ComputeFile(inputFile.value);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(30));
                {
                    GUILayout.Label("Md5", GUILayout.Width(50));

                    GUILayout.Label(message, new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontSize = 20 });

                    if (GUILayout.Button("Copy", GUILayout.Width(60)))
                    {
                        GUIUtility.systemCopyBuffer = message;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}