using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SText : InfiniteLoopItem
{
    [SerializeField] private Text text;

    protected override void Refresh()
    {
        text.text = ((int)Source).ToString();
    }

    public void Refresh(string v)
    {
        text.text = v;
    }
}
