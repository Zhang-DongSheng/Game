using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataDialog : DataBase
    {
        public string chapter;

        public uint start;

        public uint end;

        public string description;

        public List<DialogInformation> list;

        public DialogInformation Get(uint dialogID)
        {
            return list.Find(x => x.primary == dialogID);
        }

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var role = m_list[i].GetType<DialogInformation>();

                role.primary = m_list[i].GetUInt("ID");

                list.Add(role);
            }
            start = list[0].primary;

            end = list[count - 1].primary;
        }

        public override void Clear()
        {
            list = new List<DialogInformation>();
        }
    }
    [Serializable]
    public class DialogInformation : InformationBase
    {
        public string role;
        [TextArea]
        public string content;

        public uint next;

        public uint trigger;

        public uint[] options;
    }
}