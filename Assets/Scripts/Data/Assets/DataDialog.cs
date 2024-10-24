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
    }
    [Serializable]
    public class DialogInformation : InformationBase
    {
        public string role;
        [TextArea]
        public string content;

        public uint next;

        public uint[] options;
    }
}