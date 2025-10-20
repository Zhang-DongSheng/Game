using Game.Data;
using Game.Logic;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        private Language _language;

        private TMP_FontAsset _font;

        private readonly Dictionary<string, string> _words = new Dictionary<string, string>();

        public void Initialize()
        {
            _language = GlobalVariables.Get<Language>(GlobalKey.LANGUAGE);

            Load(_language, false);

            ScheduleLogic.Instance.Update(Schedule.Language);
        }

        public void Update(Language language)
        {
            if (this._language == language) return;

            this._language = language;

            GlobalVariables.Set(GlobalKey.LANGUAGE, language);

            Load(language, true);
        }

        public string Get(string key)
        {
            if (_words.TryGetValue(key.ToLower(), out string value))
            {
                return value;
            }
            return key;
        }

        public string GetByParameter(string key, params object[] parameters)
        {
            if (_words.TryGetValue(key.ToLower(), out string value))
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
                        _words.Clear();

                        foreach (var word in asset.list)
                        {
                            _words.Add(word.key, word.value);
                        }
                        _font = asset.font;
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
                    _words.Clear();

                    foreach (var word in asset.list)
                    {
                        _words.Add(word.key, word.value);
                    }
                    _font = asset.font;
                }
                else
                {
                    Debuger.LogError(Author.Data, "Language Data is Error");
                }
            }
        }

        public TMP_FontAsset Font { get { return _font; } }

        public Language Language { get { return _language; } }
    }
}