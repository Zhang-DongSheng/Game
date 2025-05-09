using UnityEngine;

namespace UnityEditor
{
    public class ApplicationListener
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Listener()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    { 
                        
                    }
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    { 
                        
                    }
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    {
                        
                    }
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    {
                        var asset = AssetUtils.Find<MaterialKeep>();

                        asset?.Keep();
                    }
                    break;
            }
        }
    }
}