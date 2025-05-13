using Game;
using UnityEngine;

namespace UnityEditor.Window
{
    public class ArtistConfig : ArtistBase
    {
        private GUIContent content;

        public override void Initialise()
        {
            var bitmap = Utility.QRCode.Create(GameConfig.GitURL, 2, 5);

            var texture = new Texture2D(bitmap.Width, bitmap.Height, TextureFormat.ARGB32, true);

            texture.LoadImage(Utility.Image.Serialize(bitmap));

            content = new GUIContent(texture, string.Empty);
        }

        public override void Refresh()
        {
            GUILayout.Box(content);

            GUILayout.Space(30);

            if (GUILayout.Button(ToLanguage("Contact us")))
            {
                Utility.Common.OpenQQ(GameConfig.QQ);
            }
            GUILayout.Label("Version:1.0.0");

            GUILayout.FlexibleSpace();
        }

        public override string Name => ToLanguage("Config");
    }
}