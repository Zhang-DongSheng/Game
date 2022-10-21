using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [DisallowMultipleComponent]
    class PrefabBind : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objects;

        private void Awake()
        {
            foreach (var go in objects)
            {
                SetActive(go, false);
            }
        }
        [ContextMenu("Bind")]
        private void Bind()
        {
            objects.Clear();

            Transform[] children = transform.GetComponentsInChildren<Transform>(true);

            foreach (var child in children)
            {
                if (!child.gameObject.activeSelf)
                {
                    objects.Add(child.gameObject);
                }
            }
        }

        private void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}