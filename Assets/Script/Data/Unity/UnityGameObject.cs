using System.Collections.Generic;
using UnityEngine;

namespace Data.Unity
{
    public struct UnityGameObject
    {
        public string name;

        public string tag;

        public int layer;

        public bool active;

        public UnityTransform transform;

        public List<UnityComponent> components;

        public static implicit operator UnityGameObject(GameObject go)
        {
            UnityGameObject unity = new UnityGameObject()
            {
                name = go.name,
                tag = go.tag,
                layer = go.layer,
                active = go.activeSelf,
                transform = go.transform,
                components = new List<UnityComponent>()
            };
            return unity;
        }
    }
}