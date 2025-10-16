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
                var steam = StreamFromString(request.downloadHandler.text);

                var document = steam.ToHtmlDocument();
                // 查找页面中的标题
                var qs = document.QuerySelector("body");

                txtContent.text = qs.TextContent;
            }
            else
            {
                Debug.LogError($"Failed to load proclamation: {request.error}");
            }
        }

        public static Stream StreamFromBytes(Byte[] content)
        {
            var stream = new MemoryStream(content) { Position = 0 };
            return stream;
        }

        public static Stream StreamFromString(String s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public override void Exit()
        {
            base.Exit();

            PopupLogic.Instance.Complete();
        }
    }
}