using System;
using System.Net;

namespace UnityEngine
{
    public class TimeSynchronization : MonoSingleton<TimeSynchronization>
    {
        private const string URL = "https://www.baidu.com";

        [SerializeField] private float interval = 60 * 30;

        private WebHeaderCollection collection;

        private WebResponse response;

        private WebRequest request;

        private DateTime time;

        private string value;

        private float second;

        private void Awake()
        {
            Synchronization();
        }

        private void Update()
        {
            second += Time.deltaTime;

            if (second > interval)
            {
                Synchronization();
            }
        }

        public DateTime Now
        {
            get
            {
                return time.AddSeconds(second);
            }
        }

        public void Synchronization()
        {
            string value = SynchronizationServer();

            if (string.IsNullOrEmpty(value))
            {
                time = DateTime.Now;
            }
            else
            {
                time = Convert.ToDateTime(value);
            }
            second = 0;
        }


        public string SynchronizationServer()
        {
            try
            {
                request = WebRequest.Create(URL);
                request.Timeout = 1000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = request.GetResponse();
                collection = response.Headers;
                foreach (var key in collection.AllKeys)
                {
                    if (key == "Date")
                    {
                        value = collection[key];
                        break;
                    }
                }
                return value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                if (request != null) { request.Abort(); }
                if (response != null) { response.Close(); }
                if (collection != null) { collection.Clear(); }
            }
        }
    }
}