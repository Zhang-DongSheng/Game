using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataTask : ScriptableObject
    {
        public List<TaskInformation> tasks = new List<TaskInformation>();
    }
    [Serializable]
    public class TaskInformation
    {
        public int ID;

        public string name;

        public string icon;

        public int count;

        public string description;

        public List<Prop> props;
    }

    public class Task
    {
        public int ID;

        public int step;

        public TaskStatus status;
    }

    public enum TaskStatus
    {
        Undone,             //未完成
        Available,          //可领取
        Received,           //已领取
    }
}