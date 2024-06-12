using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public abstract class ItemGuidanceBase : MonoBehaviour
    {
        public void SetActive(Component component, bool active)
        {
            if (component != null)
            {
                SetActive(component.gameObject, active);
            }
        }

        public void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}