using Game.Resource;
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
            if (model != null)
            {
                GameObject.Destroy(model);
            }
        }

        protected void Load(string path)
        {
            ResourceManager.LoadAsync<GameObject>(path, (prefab) =>
            {
                if (model != null)
                {
                    GameObject.Destroy(model);
                }
                model = GameObject.Instantiate(prefab, transform);

                model.transform.localPosition = Vector3.zero;
            });
        }
    }
}