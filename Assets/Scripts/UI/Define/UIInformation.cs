namespace Game.UI
{
    [System.Serializable]
    public class UIInformation
    {
        public UIPanel panel;

        public UILayer layer;

        public string name;

        public bool record;

        public int order;

        public string path;

        public UIInformation()
        {

        }

        public static UIInformation Default(UIPanel panel)
        {
            return new UIInformation()
            {
                panel = panel,

                layer = UILayer.Window,

                name = panel.ToString(),

                record = false,

                order = 0,

                path = string.Format("{0}/{1}.prefab", UIConfig.Prefab, panel.ToString())
            };
        }
    }
}