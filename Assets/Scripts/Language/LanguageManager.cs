using Data;
using UnityEngine;

namespace Game
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private Dictionary dictionary;

        public Language language { get; private set; }

        public void Initialize()
        {
            language = GlobalVariables.Get<Language>(Const.LANGUAGE);

            ScheduleLogic.Instance.Update(Schedule.Language);
        }

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

        public string GetByParameter(string key, params object[] parameters)
        {
            if (dictionary != null)
            {
                return string.Format(dictionary.Get(key), parameters);
            }
            else
            {
                Load(language, false);

                if (dictionary != null)
                {
                    return string.Format(dictionary.Get(key), parameters);
                }
                else
                {
                    return string.Format(key, parameters);
                }
            }
        }

        public void Switch(Language language)
        {
            if (this.language == language) return;

            this.language = language;

            Load(language, true);

            GlobalVariables.Set(Const.LANGUAGE, language);

            EventManager.Post(EventKey.Language);
        }

        private void Load(Language language, bool async)
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
    }
}