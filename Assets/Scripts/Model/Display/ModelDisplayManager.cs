using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    public class ModelDisplayManager : MonoBehaviour
    {
        public static ModelDisplayManager Instance { get; private set; }

        [SerializeField] private Camera _camera;

        [SerializeField] private List<ModelDisplayGroup> _groups = new List<ModelDisplayGroup>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void RefreshModel(ModelDisplayInformation model)
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
                    cell.Refresh(model);
                }
                else
                {
                    cell.Release();
                }
                cell.Empty(model != null);
            }
        }

        public void RefreshModels(int index, List<ModelDisplayInformation> list)
        {
            var group = _groups.Find(x => x.index == index);

            if (group == null) return;

            int count = group.cells.Count;

            for (int i = 0; i < count; i++)
            {
                if (group.cells[i] == null) continue;

                var cell = group.cells[i];

                var model = list.Find(x => x.index == cell.serial);

                if (model != null)
                {
                    cell.Refresh(model);
                }
                else
                {
                    cell.Release();
                }
                cell.Empty(model != null);
            }
        }

        public void SwitchGroup(params int[] parameter)
        {
            int count = _groups.Count;

            for (int i = 0; i < count; i++)
            {
                if (_groups[i] != null)
                {
                    _groups[i].SetActive(parameter.Exist(_groups[i].index));
                }
            }
        }

        public ModelDisplay GetModelDisplay(int index, int serial)
        {
            var group = _groups.Find(x => x.index == index);

            if (group == null) return null;

            var cell = group.cells.Find(x => x.serial == serial);

            if (cell == null) return null;

            return cell.Display;
        }

        public void Clear()
        {
            int count = _groups.Count;

            for (int i = 0; i < count; i++)
            {
                if (_groups[i] != null)
                {
                    foreach (var cell in _groups[i].cells)
                    {
                        if (cell != null)
                        {
                            cell.Release();
                        }
                    }
                    _groups[i].SetActive(false);
                }
            }
        }
    }
}