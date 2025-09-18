using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class TextTranslate
    {
        private const string APIKEY = "0FMI4J9BOiHVQiVdXxOGTLeQ";

        private const string SECRETKEY = "SG9e3w4FOMh4I9MeK5MlB3Rs6273KSHJ";

        private const string TOKEN = @"https://aip.baidubce.com/oauth/2.0/token?client_id={0}&client_secret={1}&grant_type=client_credentials";

        private const string URL = @"https://aip.baidubce.com/rpc/2.0/mt/texttrans/v1?access_token=";

        public void GetWorld(string key, string from, string to, Action<string> callback)
        {
            RuntimeManager.Instance.StartCoroutine(Translate(key, from, to, callback));
        }

        IEnumerator Translate(string key, string from, string to, Action<string> callback)
        {
            var url = string.Format(TOKEN, APIKEY, SECRETKEY);

            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            var token = request.downloadHandler.text;

            var json = $"?Content-Type=application/json&Accept=application/json";

            url = URL + token + json;

            request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debuger.Log(Author.Script, request.downloadHandler.text);

                callback?.Invoke(key);
            }
            else
            {
                callback?.Invoke(key);
            }
        }
    }
}