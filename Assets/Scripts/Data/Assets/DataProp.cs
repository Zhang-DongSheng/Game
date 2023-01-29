using Game;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataProp : DataBase
    {
        public List<PropInformation> props = new List<PropInformation>();

        public PropInformation Get(int key, bool quick = false)
        {
            return props.Find(x => x.primary == key);
        }

        public PropInformation Get(uint key, bool quick = false)
        {
            return props.Find(x => x.primary == key);
        }

        public override void Set(string content)
        {
            base.Set(content);
            // 一定要记得去掉最后一行的逗号
            JsonData json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                JsonData list = json.GetJson("list");

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    props.Add(new PropInformation()
                    {
                        primary = list[i].GetUInt("ID"),
                        name = list[i].GetString("name"),
                        icon = list[i].GetString("icon"),
                        category = list[i].GetInt("category"),
                        quality = list[i].GetByte("quality"),
                        price = list[i].GetFloat("price"),
                        source = list[i].GetInt("source"),
                        description = list[i].GetString("description")
                    });
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "道具DB解析失败");
            }
        }

        public override void Clear()
        {
            props = new List<PropInformation>();
        }
    }
    [System.Serializable]
    public class PropInformation : InformationBase
    {
        public string name;

        public string icon;

        public byte quality;

        public int category;

        public float price;

        public int source;

        public string description;
    }
}