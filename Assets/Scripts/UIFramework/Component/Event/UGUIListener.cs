using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UGUIListener : MonoBehaviour, IPointerClickHandler
    {
        private void Awake()
        {
            if (TryGetComponent(out Button button))
            { 
                
            }
            else if(TryGetComponent(out Toggle toggle)) 
            {
                
            }
            else if (TryGetComponent(out Slider slider))
            {

            }
            else if (TryGetComponent(out ScrollRect rect))
            {

            }
            else if (TryGetComponent(out Scrollbar bar))
            {

            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}
