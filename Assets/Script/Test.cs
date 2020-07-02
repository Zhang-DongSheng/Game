using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("a", "111");
        PlayerPrefs.SetString("b", "222");
        PlayerPrefs.SetInt("c", 123);
        PlayerPrefs.SetFloat("d", 12);
    }

    void Update()
    {
        
    }
}
