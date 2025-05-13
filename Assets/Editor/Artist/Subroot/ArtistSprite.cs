using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Window
{
    public class ArtistSprite : ArtistBase
    {
        private UnityEngine.Object select;

        public override void Initialise()
        {
            
        }

        public override void Refresh()
        {
            select = EditorGUILayout.ObjectField(select, typeof(Texture2D), false);

            if (select == null) return;

            if (select is Texture2D texture)
            {
                var max = Mathf.Max(texture.width, texture.height);

                var ratio = 100f / max;

                var width = texture.width * ratio;

                var height = texture.height * ratio;

                var rect = GUILayoutUtility.GetRect(width, height);

                EditorGUI.DrawPreviewTexture(rect, texture);

                if (!texture.isReadable)
                {
                    EditorGUILayout.HelpBox("Please set the texture read/write to true", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox(ToLanguage("Please select a texture"), MessageType.Error);
            }

            if (GUILayout.Button(ToLanguage("Gray")))
            {
                var path = AssetDatabase.GetAssetPath(select);

                path = Game.Utility.Path.NewFile(path);

                SpriteUtils.Gray(select as Texture2D, path);
            }

            if (GUILayout.Button(ToLanguage("Compress")))
            {
                var path = AssetDatabase.GetAssetPath(select);

                path = Game.Utility.Path.NewFile(path);

                SpriteUtils.Compress(select as Texture2D, path);
            }
        }

        public override string Name => ToLanguage("Sprite");
    }
}
