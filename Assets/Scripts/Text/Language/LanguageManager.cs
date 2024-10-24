using Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private Language language;

        private readonly Dictionary<string, string> words = new Dictionary<string, string>();

        public void Initialize()
        {
            language = GlobalVariables.Get<Language>(Const.LANGUAGE);

            Load(language, false);

            ScheduleLogic.Instance.Update(Schedule.Language);
        }

        public void Update(Language language)
        {
            if (this.language == language) return;

            this.language = language;

            GlobalVariables.Set(Const.LANGUAGE, language);

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
                DataManager.Instance.LoadAsync<DataLanguage>((asset) =>
                {
                    if (asset != null)
                    {
                        var information = asset.list.Find(x => x.language == language);

                        words.Clear();

                        foreach (var word in information.words)
                        {
                            words.Add(word.x, word.y);
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
                var asset = DataManager.Instance.Load<DataLanguage>();

                if (asset != null)
                {
                    var information = asset.list.Find(x => x.language == language);

                    words.Clear();

                    foreach (var word in information.words)
                    {
                        words.Add(word.x, word.y);
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