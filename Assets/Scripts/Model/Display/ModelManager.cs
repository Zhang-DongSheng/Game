using Game.Model;
using Game.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ModelManager : MonoSingleton<ModelManager>
    {
        private readonly Dictionary<string, GameObject> _models = new Dictionary<string, GameObject>();

        public void Display(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (_models.ContainsKey(path))
            {
                DisplayModel(path);
            }
            else
            {
                ResourceManager.LoadAsync<GameObject>(path, (prefab) =>
                {
                    if (_models.ContainsKey(path))
                    {
                        // 已存在，不创建
                    }
                    else
                    {
                        var clone = GameObject.Instantiate(prefab);

                        clone.transform.SetParent(transform, false);

                        clone.transform.localPosition = Vector3.zero;

                        if (clone.TryGetComponent(out ModelDisplay display))
                        {
                            display.Initialize(clone.transform);
                        }
                        _models.Add(path, clone);
                    }
                    // 
                    DisplayModel(path);
                });
            }
        }

        public void Modify(string key, string part, string index, string skin)
        {
            if (_models.TryGetValue(key, out GameObject model))
            {
                model.GetComponent<ModelDisplay>().Change(part, index, skin);
            }
        }

        public void Remove(string path)
        {
            
        }

        public void Clear()
        {

        }

        private void DisplayModel(string key)
        {
            if (_models.TryGetValue(key, out GameObject model))
            {
                model.SetActive(true);
            }
            else
            {
                Debuger.LogError(Author.Resource, "资源未加载！");
            }
        }
    }
}
