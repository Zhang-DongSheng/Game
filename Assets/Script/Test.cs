using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RenewablePool.Instance.Release();
        }
    }

    private void OnClickTest()
    {
        int index = Random.Range(0, 5);

        string key = null, url = "";

        switch (index)
        {
            case 0:
                key = "ceo";
                break;
            case 1:
                key = "deeplove";
                break;
            case 2:
                key = "indra";
                break;
            case 3:
                key = "lovebot";
                break;
            case 4:
                key = "ourhb";
                break;
        }

#if UNITY_EDITOR
        url = "win32/dongshengtest/texture/" + key;
#else
        url = "android/dongsheng/texture/" + key;
#endif
 
        Debug.LogFormat("Bundle : <color=blue>{0}</color> Key : <color=green>{1}</color>", url, key);

        asset_test.CreateAsset(url,key, callBack: (bundle) =>
        {
            Texture2D texture = bundle as Texture2D;

            if (sprite != null)
            {
                Destroy(sprite);
            }

            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            asset_test.GetComponent<Image>().sprite = sprite;

            Debug.Log("success");
        });
    }

    Sprite sprite;

    private void OnSubmit(string value)
    {
        variable = value;
    }
}
