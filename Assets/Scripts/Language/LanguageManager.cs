using Data;
using Game.Resource;
using UnityEngine;

namespace Game
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private readonly string path = "Package/Text";

        private Language language = Language.Chinese;

        private DataLanguage dictionary;

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
                
            }
            else
            {
                ResourceManager.LoadAsync(string.Format("{0}/language_{1}.txt", path, language), (value) =>
                {
                    if (value is TextAsset asset)
                    {
                        dictionary = JsonUtility.FromJson<DataLanguage>(asset.text);
                    }
                });
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