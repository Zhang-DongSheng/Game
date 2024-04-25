using Game.Model;
using Game.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ModelManager : MonoSingleton<ModelManager>
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private List<ModelGroup> _groups = new List<ModelGroup>();

        public void RefreshModel(ModelInformation model)
        {
            var group = _groups.Find(x => x.index == 1);

            if (group == null) return;

            int count = group.cells.Count;

            for (int i = 0; i < count; i++)
            {
                if (group.cells[i] == null) continue;

                var cell = group.cells[i];

                if (model != null)
                {
                    if (cell.display == null)
                    {
                        cell.Create();
                    }
                    cell.display.Refresh(model);
                }
                else
                {
                    cell.Release();
                }
                cell.empty.SetActive(cell.display != null);
            }
        }

        public void RefreshModels(int index, List<ModelInformation> list)
        {
            var group = _groups.Find(x => x.index == index);

            if (group == null) return;

            int count = group.cells.Count;

            for (int i = 0; i < count; i++)
            {
                if (group.cells[i] == null) continue;

                var cell = group.cells[i];

                var model = list.Find(x => x.index == cell.index);

                if (model != null)
                {
                    if (cell.display == null)
                    {
                        cell.Create();
                    }
                    cell.display.Refresh(model);
                }
                else
                {
                    cell.Release();
                }
                cell.empty.SetActive(cell.display != null);
            }
        }





















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

        public void Remove(string key)
        {
            foreach (var model in _models)
            {
                if (model.Key == key)
                {
                    GameObject.Destroy(model.Value);
                    break;
                }
            }
            _models.Remove(key);
        }

        public void Clear()
        {
            foreach (var model in _models.Values)
            {
                if (model != null)
                { 
                    GameObject.Destroy(model);
                }
            }
            _models.Clear();
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
