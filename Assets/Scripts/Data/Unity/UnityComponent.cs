using System.Collections.Generic;
using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public struct UnityComponent
    {
        public string name;

        public string type;

        public List<UnityParameter> parameters;

        public static implicit operator UnityComponent(Component component)
        {
            UnityComponent unity = new UnityComponent()
            {
                name = component.name,
                type = component.GetType().Name,
                parameters = new List<UnityParameter>()
            };
            return unity;
        }
    }
}