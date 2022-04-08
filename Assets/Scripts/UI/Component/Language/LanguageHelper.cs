using Data;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    public class LanguageHelper : MonoSingleton<LanguageHelper>
    {
        [SerializeField] private Language language;

        private DataText m_data;

        private Dictionary dictionary;

        private void Awake()
        {
            m_data = DataManager.Instance.Load<DataText>("Data/language");
        }

        private void Start()
        {
            LanguageSwitch();
        }

        private void OnValidate()
        {
            if (m_data != null)
            {
                if (dictionary != null)
                {
                    if (dictionary.language != language)
                    {
                        LanguageSwitch();
                    }
                }
                else
                {
                    LanguageSwitch();
                }
            }
        }

        public void LanguageSwitch()
        {
            if (m_data != null)
            {
                dictionary = m_data.Dictionary(language);
            }
            EventManager.Post(EventKey.Language, new EventMessageArgs());
        }

        public string Word(string key)
        {
            if (dictionary != null)
            {
                for (int i = 0; i < dictionary.words.Count; i++)
                {
                    if (dictionary.words[i].key == key)
                    {
                        return dictionary.words[i].value;
                    }
                }
            }
            return key;
        }

        public Font Font
        {
            get
            {
                if (dictionary != null)
                {
                    return dictionary.font;
                }
                return null;
            }
        }
    }
}