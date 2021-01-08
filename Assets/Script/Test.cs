using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Test : MonoBehaviour
    {
        public RenewableImage img_test;

        public RenewableAsset asset_test;

        public RenewableAudio audio_test;

        public Button btn_test;

        public InputField input_test;

        private string variable;

        private readonly List<string> urls = new List<string>()
    {
        "picture/cover/ceo.jpg",
        "picture/cover/ourhb.jpg",
    };

        private void Awake()
        {
            if (btn_test != null)
            {
                btn_test.onClick.AddListener(OnClickTest);
            }

            if (input_test != null)
            {
                input_test.onEndEdit.AddListener(OnSubmit);
            }
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void OnClickTest()
        {

        }

        Sprite sprite;

        private void OnSubmit(string value)
        {
            variable = value;
        }
    }
}