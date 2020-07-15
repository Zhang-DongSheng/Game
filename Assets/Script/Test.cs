using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button btn_test;

    public InputField input_test;

    private string variable;

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

    private void OnSubmit(string value)
    {
        variable = value;
    }
}
