using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Factory;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Image image;


    void Start()
    {
        Factory.Instance.Pop("Test");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Factory.Instance.Remove("Test");
        }
    }
}
