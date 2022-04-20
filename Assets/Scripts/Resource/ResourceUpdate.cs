using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Resource
{
    public class ResourceUpdate
    {
        public static void Direction()
        {
            //Utility.MD5.ComputeFile()
        }

        IEnumerator Download(string path)
        {
            UnityWebRequest request = UnityWebRequest.Get(ServerUrl(path));

            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    {

                    }
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Write(LocalPath(path), request.downloadHandler.data);
                    }
                    break;
                default:
                    {
                        Debuger.LogError(Author.Resource, request.error);
                    }
                    break;
            }
        }

        private string Read(string path)
        {
            return Utility.Document.Read(path);
        }

        private void Write(string path, byte[] buffer)
        {
            Utility.Document.Write(path, buffer);
        }

        private string ServerUrl(string path)
        {
            return string.Format("{0}/{1}", GameConfig.ResourceServerURL, path);
        }

        private string LocalPath(string path)
        {
            return string.Format("{0}/{1}", Application.persistentDataPath, path);
        }
    }
}