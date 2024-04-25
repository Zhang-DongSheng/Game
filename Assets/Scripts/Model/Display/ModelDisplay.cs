using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public class ModelDisplay : ItemBase
    {
        private readonly Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> _parts = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();

        public void Initialize(Transform target)
        {
            _parts.Clear();

            var parts = target.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var part in parts)
            {
                string[] split = part.name.Split('-');

                string key = split[0];

                string index = split.Length > 1 ? split[1] : "-1";

                if (_parts.ContainsKey(key))
                {
                    _parts[key].Add(index, part);
                }
                else
                {
                    _parts.Add(key, new Dictionary<string, SkinnedMeshRenderer>() { { index, part } });
                }
            }
        }

        public void Refresh(ModelInformation information)
        { 
            
        }

        public void Change(string part, string index, string skin)
        {
            foreach (var parts in _parts)
            {
                if (parts.Key == part)
                {
                    foreach (var p in parts.Value)
                    {
                        bool active = p.Key == index;

                        if (active)
                        {
                            
                        }
                        SetActive(p.Value, active);
                    }
                    break;
                }
            }
        }
    }
}