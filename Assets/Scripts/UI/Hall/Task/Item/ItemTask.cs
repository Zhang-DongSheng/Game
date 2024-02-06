using Data;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemTask : ItemBase
    {
        [SerializeField] private Text txtDescription;

        [SerializeField] private Button btnSource;

        [SerializeField] private Button btnGet;

        [SerializeField] private ItemStatus status;

        [SerializeField] private ItemPropGroup group;

        private uint taskID;

        protected override void OnAwake()
        {
            btnSource.onClick.AddListener(OnClickSource);

            btnGet.onClick.AddListener(OnClickGet);
        }

        public void Refresh(Task task)
        {
            taskID = task.parallelism;

            var table = DataTask.Get(task.parallelism);

            txtDescription.text = table.description;

            group.Refresh(table.rewards.Count, (index, item) =>
            {
                item.Refresh(table.rewards[index].x, (int)table.rewards[index].y);
            });
            status.Refresh(task.status);
        }

        private void OnClickSource()
        {
            var table = DataTask.Get(taskID);

            TaskLogic.Goto(table.type);
        }

        private void OnClickGet()
        {
            TaskLogic.Instance.RequestGetTaskRewards(taskID);
        }
    }
}