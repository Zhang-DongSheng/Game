using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemTask : ItemBase
    {
        [SerializeField] private ItemStatus status;

        public void Refresh(Task task)
        {
            status.Refresh(task.status);
        }
    }
}