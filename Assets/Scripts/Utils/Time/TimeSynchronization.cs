using System;
using System.Net;
using UnityEngine;

namespace Game
{
    public class TimeSynchronization : MonoSingleton<TimeSynchronization>
    {
        private const string URL = "http://www.baidu.com";

        private readonly DateTime Base = new DateTime(621355968000000000);

        [SerializeField] private float interval = 60 * 30;

        private WebHeaderCollection collection;

        private WebResponse response;

        private WebRequest request;

        private DateTime time = DateTime.UtcNow;

        private TimeSpan span;

        private string value;

        private float second;

        private int day;

        public Action action;

        private void Start()
        {
            Synchronization(); day = Now.DayOfYear;
        }

        private void Update()
        {
            second += Time.deltaTime;

            if (second > interval)
            {
                Synchronization();
            }

            if (day != Now.DayOfYear)
            {
                day = Now.DayOfYear; Tomorrow();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            Synchronization();
        }

        private void Synchronization()
        {
            string value = SynchronizationServer();

            if (string.IsNullOrEmpty(value))
            {
                time = DateTime.UtcNow;
            }
            else
            {
                time = Convert.ToDateTime(value).ToUniversalTime();
            }
            second = 0;
        }

        private string SynchronizationServer()
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

        private void Tomorrow()
        {
            action?.Invoke();
        }

        public DateTime Now
        {
            get
            {
                return time.AddSeconds(second);
            }
        }

        public float Remaining(long ticks)
        {
            span = Base.AddMilliseconds(ticks) - Now;

            return (float)span.TotalSeconds;
        }
    }
}