using Data;
using UnityEngine;

namespace Game
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private LanguageInformation information;

        private Language language;

        public void Initialize()
        {
            language = GlobalVariables.Get<Language>(Const.LANGUAGE);

            Load(language, false);

            ScheduleLogic.Instance.Update(Schedule.Language);
        }

        public void Refresh(Language language)
        {
            if (this.language == language) return;

            this.language = language;

            GlobalVariables.Set(Const.LANGUAGE, language);

            Load(language, true);
        }

        public string Get(string key)
        {
            if (information != null)
            {
                return information.dictionary.Get(key);
            }
            else
            {
                return key;
            }
        }

        public string GetByParameter(string key, params object[] parameters)
        {
            if (information != null)
            {
                return string.Format(information.dictionary.Get(key), parameters);
            }
            else
            {
                return string.Format(key, parameters);
            }
        }

        private void Load(Language language, bool async)
        {
            if (async)
            {
                DataManager.Instance.LoadAsync<DataLanguage>((asset) =>
                {
                    if (asset != null)
                    {
                        information = asset.languages.Find(x => x.language == language);
                    }
                    else
                    {
                        Debuger.LogError(Author.Data, "Language Data is Error");
                    }
                    EventManager.Post(EventKey.Language);
                });
            }
            else
            {
                var asset = DataManager.Instance.Load<DataLanguage>();

                if (asset != null)
                {
                    information = asset.languages.Find(x => x.language == language);
                }
                else
                {
                    Debuger.LogError(Author.Data, "Language Data is Error");
                }
            }
        }

        public Language Language { get { return language; } }
    }
}