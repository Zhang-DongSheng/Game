using UnityEngine;

namespace UnityEditor.Window
{
    public abstract class ArtistBase
    {
        public abstract string Name { get; }

        public abstract void Initialise();

        public abstract void Refresh();

        public virtual void ShowNotification(string message)
        {
            Artist artist = EditorWindow.GetWindow<Artist>();

            if (artist != null)
            {
                artist.ShowNotification(new GUIContent(message));
            }
        }
    }
}