using LitJson;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class GoogleTranslate : Singleton<GoogleTranslate>
    {
        const string GOOGLE = "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}";

        public void GetWorld(string key, string from, string to, Action<string> callback)
        {
            RuntimeManager.Instance.StartCoroutine(Translate(key, from, to, callback));
        }

        IEnumerator Translate(string key, string from, string to, Action<string> callback)
        {
            var url = string.Format(GOOGLE, from, to, key);

            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debuger.Log(Author.Script, request.downloadHandler.text);

                var json = JsonMapper.ToObject(request.downloadHandler.text);

                var content = json[0].ToString();

                callback?.Invoke(content);
            }
            else
            {
                callback?.Invoke(key);
            }
        }
    }
}