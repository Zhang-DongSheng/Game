using LitJson;
using System;
using System.Collections.Generic;

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

        public override void Load(JsonData json)
        {
            int count = json.Count;

            for (int i = 0; i < count; i++)
            {
                var dialog = json[i].GetType<DialogInformation>();

                dialog.primary = json[i].GetUInt("ID");

                dialog.type = json[i].GetEnum<DialogType>("type");

                list.Add(dialog);
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
        public DialogType type;

        public string role;

        public string content;

        public string parameter;

        public uint next;
    }
    /// <summary>
    /// 对话类型
    /// </summary>
    public enum DialogType
    {
        /// <summary>
        /// 对话
        /// </summary>
        Content = 1,
        /// <summary>
        /// 选项
        /// </summary>
        Option = 2,
        /// <summary>
        /// 玩家
        /// </summary>
        Player = 3,
        /// <summary>
        /// 背景
        /// </summary>
        Background = 4,
    }
}