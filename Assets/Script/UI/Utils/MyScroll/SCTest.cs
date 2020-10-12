using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCTest : InfiniteLoopItem
{
    public Text txt_label;

    protected override void Refresh()
    {
        txt_label.text = Source.ToString();
    }
}