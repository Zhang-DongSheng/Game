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

        public UIInformation() { }

        public static UIInformation Default(UIPanel panel)
        {
            return new UIInformation()
            {
                panel = (int)panel,

                layer = UILayer.Window,

                type = UIType.Panel,

                name = panel.ToString(),

                destroy = false,

                order = 0,

                path = string.Format("{0}/{1}.prefab", UIDefine.Prefab, panel)
            };
        }
    }
}