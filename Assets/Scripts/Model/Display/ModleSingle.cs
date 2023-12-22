using Game.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public class ModleSingle : MonoBehaviour
    {
        private string display;

        private GameObject model;

        private Action<string, GameObject> callback;

        private readonly Dictionary<string, GameObject> _models = new Dictionary<string, GameObject>();

        private void Display()
        {
            if (_models.ContainsKey(display))
            {
                foreach (var _model in _models)
                {
                    if (_model.Key == display)
                    {
                        _model.Value.SetActive(true);
                    }
                    else
                    {
                        _model.Value.SetActive(false);
                    }
                }
                model = _models[display];
            }
            else
            {
                Debuger.LogError(Author.Resource, "资源未加载！");
            }
        }

        public void Display(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (display == path) return;

            display = path;

            if (_models.ContainsKey(display))
            {
                Display();
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

                        callback?.Invoke(path, clone);

                        _models.Add(path, clone);
                    }
                    // 
                    Display();
                });
            }
        }

        public void Clear()
        { 
            
        }
    }
}