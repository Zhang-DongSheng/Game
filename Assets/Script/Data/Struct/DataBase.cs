using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public abstract class DataBase : ScriptableObject
    {
        protected readonly Dictionary<int, int> index = new Dictionary<int, int>();

        protected T QuickLook<T>(List<T> list, int identification) where T : InformationBase
        {
            return null;
        }
    }

    public class InformationBase
    {
        public int identification;
    }
}