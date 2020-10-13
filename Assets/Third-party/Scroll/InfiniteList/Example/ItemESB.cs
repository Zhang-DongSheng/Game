using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class ItemESB : MonoBehaviour
    {
        public Text txt_label;

        public void Refresh(string value)
        {
            txt_label.text = value;
        }
    }
}