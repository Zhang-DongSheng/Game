namespace Game.UI
{
    [System.Serializable]
    public class UIInformation
    {
        public UIPanel panel;

        public UILayer layer;

        public UIType type;

        public string name;

        public int order;

        public string path;

        public bool destroy;

        public UIInformation()
        {

        }

        public void Copy(UIInformation source)
        {
            panel = source.panel;

            layer = source.layer;

            type = source.type;

            name = source.name;

            order = source.order;

            path = source.path;

            destroy = source.destroy;
        }

        public static UIInformation Default(UIPanel panel)
        {
            return new UIInformation()
            {
                panel = panel,

                layer = UILayer.Window,

                type = UIType.Panel,

                name = panel.ToString(),

                destroy = false,

                order = 0,

                path = string.Format("{0}/{1}.prefab", UIConfig.Prefab, panel.ToString())
            };
        }
    }
}