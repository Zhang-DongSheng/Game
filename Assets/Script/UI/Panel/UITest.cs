using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public List<RectTransform> parents;

    public List<RectTransform> childs;






    void Start()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            Text text = childs[i].GetComponentInChildren<Text>();

            float width =  text.SetTextAndGetWidth(Random.Range(0, int.MaxValue).ToString(), 10);

            childs[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
        UILayout.Horizontal(parents, childs, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
