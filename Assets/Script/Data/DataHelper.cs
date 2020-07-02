using Data;

namespace UnityEngine
{
    public class DataHelper : MonoSingleton<DataHelper>
    {
        public string Word(string key)
        {
            string _word = string.Empty;

            if (Data_Language != null)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _word = Data_Language.Word(key);
                }
            }

            return _word;
        }

        public Data_Language Data_Language
        {
            get
            {
                return DataManager.Instance.Load<Data_Language>("Language", "Data/Language");
            }
        }
    }
}