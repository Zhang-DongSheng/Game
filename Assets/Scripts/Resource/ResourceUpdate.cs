using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Resource
{
    public class ResourceUpdate : Singleton<ResourceUpdate>
    {
        private readonly List<Task> tasks = new List<Task>();

        private int count;

        private int loading;

        public void Direction(LoadingType type)
        {
            if (type != LoadingType.AssetBundle)
            {
                ScheduleLogic.Instance.Update(Schedule.Resource);
            }
            else
            {
                RuntimeManager.Instance.StartCoroutine(Download(string.Format("{0}/{1}", ResourceConfig.CloudResources, ResourceConfig.Record)));
            }
        }

        private IEnumerator Download(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    {

                    }
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        tasks.Clear();

                        string content = Encoding.Default.GetString(request.downloadHandler.data);

                        content = content.Replace("\n\r", "^");

                        string[] lines = content.Split('^');

                        count = lines.Length;

                        for (int i = 0; i < count; i++)
                        {
                            if (!string.IsNullOrEmpty(lines[i]) && lines[i].Contains("|"))
                            {
                                string[] parameter = lines[i].Split('|');

                                if (parameter.Length > 1)
                                {
                                    tasks.Add(new Task(parameter[0], parameter[1]));
                                }
                            }
                        }
                        Next();
                    }
                    break;
                default:
                    {
                        Debuger.LogError(Author.Resource, request.error);
                        //版本校对资源失败直接进入游戏
                        ScheduleLogic.Instance.Update(Schedule.Resource);
                    }
                    break;
            }
        }

        private void Next()
        {
            loading = 0;

            int count = tasks.Count;

            for (int i = count - 1; i > -1; i--)
            {
                switch (tasks[i].status)
                {
                    case Status.Loading:
                        loading++;
                        break;
                    case Status.Complete:
                        tasks.RemoveAt(i);
                        break;
                }
            }

            if (loading >= 3) return;

            count = tasks.Count;

            for (int i = 0; i < count; i++)
            {
                switch (tasks[i].status)
                {
                    case Status.Ready:
                        {
                            RuntimeManager.Instance.StartCoroutine(Download(tasks[i]));
                            loading++;
                        }
                        break;
                }
                if (loading >= 3) break;
            }

            if (count == 0)
            {
                ScheduleLogic.Instance.Update(Schedule.Resource);
            }
            else
            {
                Debuger.Log(Author.Resource, $"资源更新进度{count}/{this.count}");
            }
        }

        private IEnumerator Download(Task task)
        {
            task.status = Status.Loading;

            UnityWebRequest request = UnityWebRequest.Get(task.url);

            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    {

                    }
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        task.status = Status.Complete;

                        Utility.Document.Write(task.path, request.downloadHandler.data);

                        Next();
                    }
                    break;
                default:
                    {
                        task.status = Status.Fail;

                        Debuger.LogError(Author.Resource, request.error);

                        Next();
                    }
                    break;
            }
        }

        class Task
        {
            public string key;

            public string path;

            public string url;

            public string md5;

            public Status status;

            public Task(string key, string md5)
            {
                this.key = key;

                path = string.Format("{0}/{1}", ResourceConfig.LocalResources, key);

                url = string.Format("{0}/{1}", ResourceConfig.CloudResources, key);

                this.md5 = md5;

                if (Utility.Md5.ComputeFile(path) != md5)
                {
                    status = Status.Ready;
                }
                else
                {
                    status = Status.Complete;
                }
            }
        }

        enum Status
        {
            Ready,
            Loading,
            Fail,
            Complete,
        }
    }
}