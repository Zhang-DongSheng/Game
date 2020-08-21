using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRenewableTest : UIBase
{
    public Transform parent_image;

    public Transform parent_asset;

    public Button btn_image;

    public Button btn_asset;

    private readonly List<RenewableAsset> ren_asset = new List<RenewableAsset>();

    private readonly List<RenewableImage> ren_image = new List<RenewableImage>();

    private readonly List<string> urls = new List<string>()
    {
        "android/dongsheng/texture//ceo.jpg",
        "android/dongsheng/texture//deeplove.jpg",
        "android/dongsheng/texture//indra.jpg",
        "android/dongsheng/texture//lovebot.jpg",
        "android/dongsheng/texture//ourhb.jpg",
        "android/dongsheng/texture//indra.jpg",
        "android/dongsheng/texture//indra.jpg",
    };

    private void Awake()
    {
        RenewableResource.Instance.Init();

        btn_image.onClick.AddListener(OnClickImage);
        btn_asset.onClick.AddListener(OnClickAsset);

        RenewableAsset[] cra = parent_asset.GetComponentsInChildren<RenewableAsset>();

        for (int i = 0; i < cra.Length; i++)
        {
            ren_asset.Add(cra[i]);
        }

        RenewableImage[] cri = parent_image.GetComponentsInChildren<RenewableImage>();

        for (int i = 0; i < cri.Length; i++)
        {
            ren_image.Add(cri[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RenewablePool.Instance.Release();
        }
    }

    private int index_image;

    private void OnClickImage()
    {
        if (index_image < urls.Count)
        {
            ren_image[index_image].SetImage(urls[index_image++]);
        }
    }

    private void OnClickAsset()
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

url = "android/dongsheng/asset/" + key;

        //Debug.LogError("Current:" + url);

        for (int i = 0; i < ren_asset.Count; i++)
        {
            ren_asset[i].CreateAssetImmediate(url, "", key);
        }
    }


}
