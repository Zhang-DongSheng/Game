using System;

namespace Game.Model
{
    [Serializable]
    public class ModelDisplayInformation
    {
        public int index;

        public string path;

        public bool Equals(ModelDisplayInformation other)
        {
            if (other == null) return false;

            return other.path == path;
        }
    }
}