using Game.UI;
using System.Collections.Generic;

namespace Data
{
    public class DataUI : DataBase
    {
        public List<UIInformation> list = new List<UIInformation>();

        public static UIInformation Get(int panel)
        {
            var data = DataManager.Instance.Load<DataUI>();

            if (data != null)
            {
                return data.list.Find(x => x.panel == panel);
            }
            return null;
        }
    }
}