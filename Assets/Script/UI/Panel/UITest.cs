using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Image image;


    void Start()
    {

        RenewableUtils.SetImage(image, "andasset/picture/icon/ceo");

        //asset.CreateAsset("andasset/picture/icon/ceo", "ceo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
