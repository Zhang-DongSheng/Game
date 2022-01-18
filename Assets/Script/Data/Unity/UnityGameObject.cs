using System.Collections.Generic;

namespace Data.Unity
{
    public struct UnityGameObject
    {
        public string name;

        public bool active;

        public UnityTransform transform;

        public List<UnityComponent> components;
    }
}