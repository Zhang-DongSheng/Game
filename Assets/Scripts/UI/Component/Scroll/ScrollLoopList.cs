using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ScrollLoopList : MonoBehaviour
    {
        [SerializeField] private RectTransform content;

        [SerializeField] private ScrollRect scroll;

        [SerializeField] private GameObject prefab;

        private void Awake()
        {
            scroll.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(Vector2 vector)
        { 
            
        }
    }
}