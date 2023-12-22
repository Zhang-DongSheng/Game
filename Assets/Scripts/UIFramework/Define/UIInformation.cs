namespace Game.UI
{
    [System.Serializable]
    public class UIInformation
    {
        public int panel;

        public UIType type;

        public UILayer layer;

        public uint order;

        public string name;

        public string path;

        public bool destroy;

        public UIInformation()
        {
        
        }
    }
}