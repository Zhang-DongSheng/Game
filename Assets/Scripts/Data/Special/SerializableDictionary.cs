using System.Collections.Generic;
using UnityEngine;

namespace Data.Serializable
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] protected List<SerializablePair<TKey, TValue>> list = new List<SerializablePair<TKey, TValue>>();

        public bool Serialize { get; set; }

        public void OnBeforeSerialize()
        {
            if (Serialize)
            {
                list.Clear();

                foreach (var pair in this)
                {
                    list.Add(new SerializablePair<TKey, TValue>(pair.Key, pair.Value));
                }
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            int count = Mathf.Max(0, list.Count);

            for (int i = 0; i < count; i++)
            {
                try
                {
                    this.Add(list[i].key, list[i].value);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
    [System.Serializable]
    public class SerializablePair<TKey, TValue>
    {
        public TKey key;

        public TValue value;

        public SerializablePair(TKey key, TValue value)
        {
            this.key = key;

            this.value = value;
        }
    }
}