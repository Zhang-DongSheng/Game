using Game.Data;
using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private Language language;

        private readonly Dictionary<string, string> words = new Dictionary<string, string>();

        public void Initialize()
        {
            language = GlobalVariables.Get<Language>(GlobalKey.LANGUAGE);

            Load(language, false);

            ScheduleLogic.Instance.Update(Schedule.Language);
        }

        public void Update(Language language)
        {
            if (this.language == language) return;

            this.language = language;

            GlobalVariables.Set(GlobalKey.LANGUAGE, language);

            Load(language, true);
        }

        public string Get(string key)
        {
            if (words.TryGetValue(key.ToLower(), out string value))
            {
                return value;
            }
            return key;
        }

        public string GetByParameter(string key, params object[] parameters)
        {
            if (words.TryGetValue(key.ToLower(), out string value))
            {
                return string.Format(value, parameters);
            }
            return string.Format(key, parameters);
        }

        private void Load(Language language, bool async)
        {
            if (async)
            {
                DataManager.Instance.LoadBranchAsync<DataLanguage>(language.ToString(), (asset) =>
                {
                    if (asset != null)
                    {
                        words.Clear();

                        foreach (var word in asset.list)
                        {
                            words.Add(word.key, word.value);
                        }
                    }
                    else
                    {
                        Debuger.LogError(Author.Data, "Language Data is Error");
                    }
                    EventDispatcher.Post(UIEvent.Language);
                });
            }
            else
            {
                var asset = DataManager.Instance.LoadBranch<DataLanguage>(language.ToString());

                if (asset != null)
                {
                    words.Clear();

                    foreach (var word in asset.list)
                    {
                        words.Add(word.key, word.value);
                    }
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