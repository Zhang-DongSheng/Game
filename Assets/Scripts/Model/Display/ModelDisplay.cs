using Game.Pool;
using UnityEngine;

namespace Game.Model
{
    public class ModelDisplay : ItemBase
    {
        protected ModelDisplayInformation information;

        protected GameObject model;

        public void Refresh(ModelDisplayInformation information)
        {
            if (this.information != null && this.information.Equals(information)) return;

            this.information = information;

            Load(information.path);
        }

        public void Release()
        {
            PoolManager.Instance.Push(information.path, model);
        }

        protected void Load(string path)
        {
            PoolManager.Instance.Pop(path, (obj) =>
            {
                if (model != null)
                {
                    PoolManager.Instance.Push(path, model);
                }
                model = obj;
                model.transform.SetParent(transform);
                model.transform.localPosition = Vector3.zero;
            });
        }
    }
}