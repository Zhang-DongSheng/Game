using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCView : MonoBehaviour
{
    public InfiniteLoopScroll scroll;

    public Button btn_front;

    public Button btn_back;

    private void Awake()
    {
        scroll.onValueChanged = OnValueChanged;

        btn_front.onClick.AddListener(OnClickFront);

        btn_back.onClick.AddListener(OnClickBack);
    }

    private void Start()
    {
        scroll.Refresh(new List<int>() { 1, 2, 3, 4, 5 });
    }

    private void OnClickFront()
    {
        scroll.Front();
    }

    private void OnClickBack()
    {
        scroll.Back();
    }

    private void OnValueChanged(int index, object data)
    {
        Debug.LogWarning(index + " : " + data);
    }
}