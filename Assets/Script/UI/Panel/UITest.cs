using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Factory;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITest : UIBase
    {
        public RawImage image;

        public Button btn_next;

        private void Awake()
        {
            btn_next.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            GameLogic.Instance.model.Show();

            image.texture = GameLogic.Instance.model.Texture;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                UIQuickEntry.OpenUIConfirm("A","B",()=>
                {
                    Debug.LogError(Random.Range(0, 100));
                });
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                UIQuickEntry.OpenUINotice("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA..");
            }
        }

        private void OnClick()
        {
            //UIManager.Instance.Close(UIKey.UITest);

            GameLogic.Instance.model.Hide();

            UIManager.Instance.Open(UIKey.UILogin);
        }
    }
}