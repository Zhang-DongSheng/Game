namespace Game.Resource
{
    public static class ResourceConfig
    {
        public static string Path(LoadType type, string path)
        {
            switch (type)
            {
                case LoadType.Resources:
                    return Utility.Path.GetPathWithoutExtension(path);
                case LoadType.AssetBundle:
                    {
                        path = Utility.Path.GetPathWithoutExtension(path).ToLower();
                    }
                    return string.Format("{0}/{1}", GameConfig.Local_Resource, path);
                case LoadType.AssetDatabase:
                    return string.Format("Assets/{0}", path);
                default:
                    return path;
            }
        }
    }
}