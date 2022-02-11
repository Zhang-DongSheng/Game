using System.Collections.Generic;
using UnityEngine;

namespace Data.Unity
{
    public struct UnityComponent
    {
        public string name;

        public string type;

        public Dictionary<string, object> parameters;

        public static implicit operator UnityComponent(Component component)
        {
            UnityComponent unity = new UnityComponent()
            {
                name = component.name,
                type = component.GetType().Name,
                parameters = new Dictionary<string, object>()
            };
            return unity;
        }
    }
}