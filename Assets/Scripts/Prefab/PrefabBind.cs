using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [DisallowMultipleComponent]
    public class PrefabBind : ItemBase
    {
        [SerializeField] private List<GameObject> objects;

        protected override void OnAwake()
        {
            Execute();
        }

        public void Execute()
        {
            foreach (var go in objects)
            {
                SetActive(go, false);
            }
        }
    }
}