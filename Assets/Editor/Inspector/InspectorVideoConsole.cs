using Game;
using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomEditor(typeof(VideoConsole))]
    public class InspectorVideoConsole : Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            var target = this.target as VideoConsole;

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("换源"))
                {
                    target.Switch();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("播放"))
                {
                    target.Play();
                }
                else if (GUILayout.Button("暂停"))
                {
                    target.Pause();
                }
                else if (GUILayout.Button("停止"))
                {
                    target.Stop();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}