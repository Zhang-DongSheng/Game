using Game.UI;
using System.Collections.Generic;

namespace Data
{
    public class DataUI : DataBase
    {
        public List<UIInformation> list = new List<UIInformation>();

        public UIInformation Get(UIPanel key)
        {
            if (list != null && list.Count > 0)
            {
                foreach (var information in list)
                {
                    if (information.panel == key)
                    {
                        return information;
                    }
                }
            }
            return UIInformation.New(key);
        }
    }
}
