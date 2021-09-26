using System;
using System.Net.Http;

namespace UnityEditor.Ebook
{
    public class Downloader
    {
        public Action<string> onSuccess;

        public Action onFail;

        private readonly HttpClient client = new HttpClient();

        public async void Download(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                onSuccess?.Invoke(content);
            }
            catch
            {
                onFail?.Invoke();
            }
        }
    }
}