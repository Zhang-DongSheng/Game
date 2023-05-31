using Data;
using UnityEngine;

namespace Game
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private Language language = Language.Chinese;

        private Dictionary dictionary;

        public string Get(string key)
        {
            if (dictionary != null)
            {
                return dictionary.Get(key);
            }
            else
            {
                Load(language, false);

                if (dictionary != null)
                {
                    return dictionary.Get(key);
                }
                else
                {
                    return key;
                }
            }
        }

        public void Load(Language language, bool async)
        {
            if (async)
            {
                DataManager.Instance.LoadAsync<DataLanguage>((asset) =>
                {
                    dictionary = asset.Get(language);
                });
            }
            else
            {
                var asset = DataManager.Instance.Load<DataLanguage>();

                dictionary = asset.Get(language);
            }
        }

        public void Switch(Language language)
        {
            if (this.language == language) return;

            this.language = language;

            Load(language, true);

            EventManager.Post(EventKey.Language);
        }

        public Language Current {  get { return this.language; } }
    }
}