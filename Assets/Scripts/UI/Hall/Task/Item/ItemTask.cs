using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemTask : ItemBase
    {
        [SerializeField] private Text text;

        [SerializeField] private ItemStatus status;

        protected override void OnRegister()
        {
            
        }

        public void Refresh(Task task)
        {
            var table = DataManager.Instance.Load<DataTask>().Get(task.parallelism);

            text.text = table != null ? table.description : "null";

            status.Refresh(task.status);
        }

        private void OnClick()
        { 
            
        }
    }
}