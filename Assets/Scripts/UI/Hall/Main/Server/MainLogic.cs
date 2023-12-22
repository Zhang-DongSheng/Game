using Data;
using Game.Model;
using UnityEngine;

namespace Game
{
    public class MainLogic : Singleton<MainLogic>, ILogic
    {
        private ModelDisplay display;

        private ModleSingle model;

        public void Initialize() { }

        public void Release() { }

        public void Relevance()
        {
            display = GameObject.FindObjectOfType<ModelDisplay>(true);

            model = display.GetComponentInChildren<ModleSingle>();

            if (display != null)
            {
                Debuger.LogError(Author.UI, "展示模型");
            }
        }

        public void Display(uint roleID)
        {
            if (display == null || model == null) return;

            var table = DataManager.Instance.Load<DataRole>().Get(roleID);

            if (table == null) return;

            model.Display(table.path);

            Debuger.LogError(Author.Resource, $"创建{table.name}");
        }


        public void Clear()
        {
            if (display != null)
            {
                if (model != null)
                {
                    model.Clear();
                }
                model = null;
            }
            display = null;
        }
    }
}
