using Game.Logic;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Game.UI
{
    public class ProclamationView : ViewBase
    {
        [SerializeField] private string url;

        [SerializeField] private Text txtContent;

        [SerializeField] private Button btnConfirm;

        protected override void OnAwake()
        {
            btnConfirm.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter paramter)
        {
            StartCoroutine(LoadContent());
        }

        private IEnumerator LoadContent()
        { 
            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var document = request.downloadHandler.text.ToHtmlDocument();
                // 查找页面中的标题
                var qs = document.QuerySelector("body");

                txtContent.text = qs.TextContent;
            }
            else
            {
                Debug.LogError($"Failed to load proclamation: {request.error}");
            }
        }

        public override void Exit()
        {
            base.Exit();

            PopupLogic.Instance.Complete();
        }
    }
}