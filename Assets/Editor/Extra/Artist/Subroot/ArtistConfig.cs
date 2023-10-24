using UnityEngine;

namespace UnityEditor.Window
{
    public class ArtistConfig : ArtistBase
    {
        public override void Initialise()
        {
        
        }

        public override void Refresh()
        {
            GUILayout.Label("Version:1.0.0");
        }

        public override string Name => "Config";
    }
}