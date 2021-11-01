using Game;
using Job;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Game.Test
{
    public class Test : MonoBehaviour
    {
        public List<Transform> list;

        public Vector3 position;

        private void Awake()
        {
            
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Start()
        {
            var _ = StartAsync();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClick();
            }
        }
        /// <summary>
        /// 测试模块
        /// </summary>
        /// <param name="paramters">参数</param>
        public static void Startover(params string[] paramters)
        {
            
        }
        /// <summary>
        /// 点击测试
        /// </summary>
        public void OnClick()
        {
            
        }
        /// <summary>
        /// 菜单栏测试
        /// </summary>
        [ContextMenu("Test")]
        public void OnClickContextMenu()
        {
            
        }

        private async Task StartAsync()
        {
            string url = "Assets/Package/Prefab/UI/Panel/UIMMORPG.prefab";

            var handle = Addressables.LoadAssetAsync<UnityEngine.Object>(url);

            await handle.Task;

            if (handle.IsDone)
            {
                Debug.LogError(handle.Result.name);
            }
        }
    }

    public class TestClass : IWeight
    {
        public string name;

        public float weight;

        public string XX
        {
            get; set;
        }
        public float Weight { get { return weight; } }

        public void Do()
        {
            Debug.LogError(name + "[  ]" + XX);
        }
    }
}