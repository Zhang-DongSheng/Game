using Game.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 快照
    /// </summary>
    public class CameraSnapshot : MonoSingleton<CameraSnapshot>
    {
        [SerializeField] private new Camera camera;

        [SerializeField] private LayerMask layer;

        [SerializeField] private int width = 512;

        [SerializeField] private int height = 512;

        [SerializeField] private Transform node;

        private RenderTexture texture;

        private readonly List<SnapshotTask> tasks = new List<SnapshotTask>();

        private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        private void Awake()
        {
            transform.position = new Vector3(10000, 10000, 10000);

            node = new GameObject("Node").transform;

            node.SetParent(transform, false);

            node.localPosition = Vector3.zero;

            camera = new GameObject("Camer").AddComponent<Camera>();

            camera.transform.SetParent(transform, false);

            camera.transform.localPosition = new Vector3(0, 5, -10);

            camera.transform.localRotation = Quaternion.Euler(24, 0, 0);

            camera.clearFlags = CameraClearFlags.SolidColor;

            camera.cullingMask = LayerMask.GetMask("Default");

            texture = new RenderTexture(width, height, 1, RenderTextureFormat.ARGB32);

            camera.targetTexture = texture;

            camera.enabled = false;

            DontDestroyOnLoad(this.gameObject);
        }

        private void Next()
        { 
            int count = tasks.Count;

            if (count == 0) return;

            for (int i = 0; i < count; i++)
            {
                if (tasks[i].state != 0)
                {
                    return;
                }
            }
            Loading(tasks[0]);
        }

        private void Loading(SnapshotTask task)
        {
            task.state = 1;

            ResourceManager.LoadAsync<GameObject>(task.key, (go) =>
            {
                if (go == null)
                {
                    Release(task);
                }
                else
                {
                    task.instance = Instantiate(go, node);
                    task.instance.transform.localPosition = Vector3.zero;
                    task.instance.transform.localRotation = Quaternion.identity;
                    StartCoroutine(Capture(task.key, task));
                }
            });
        }

        private void Release(SnapshotTask task)
        { 
            var index = tasks.FindIndex(x => x.key == task.key);

            if (index > -1)
            {
                if (tasks[index].instance != null)
                {
                    Destroy(tasks[index].instance);
                }
                tasks.RemoveAt(index);
            }
            Next();
        }

        IEnumerator Capture(string key, SnapshotTask task)
        {
            camera.enabled = true;

            RenderTexture active = RenderTexture.active;

            var texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);

            camera.Render();

            RenderTexture.active = texture;

            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            texture2D.Apply();

            RenderTexture.active = active;

            if (textures.ContainsKey(key))
            {
                textures[key] = texture2D;
            }
            else
            {
                textures.Add(key, texture2D);
            }
            task.callback?.Invoke(texture2D);

            camera.enabled = false;

            yield return new WaitForEndOfFrame();

            Release(task);
        }

        public void GetTexture(string key, Action<Texture> callback)
        {
            if (textures.TryGetValue(key, out var texture))
            {
                callback?.Invoke(texture);
            }
            else
            {
                int index = tasks.FindIndex(x => x.key == key);

                if (index > -1)
                {
                    tasks[index].callback += callback;
                }
                else
                {
                    tasks.Add(new SnapshotTask()
                    {
                        key = key,
                        callback = callback
                    });
                }
            }
            Next();
        }

        class SnapshotTask
        {
            public string key;

            public int state;

            public GameObject instance;

            public Action<Texture> callback;
        }
    }
}