using Data;
using Game.UI;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public class UIPSD : MonoBehaviour
    {
        [SerializeField] private DataPSD source;

        [SerializeField] private ParentAndPrefab prefab;

        private readonly List<ItemPSD> items = new List<ItemPSD>();

        public PSDInformation config { get; set; }

        private void Awake()
        {
            config = source.First;

            Refresh();
        }

        public void Refresh()
        {
            if (config == null) return;

            for (int i = 0; i < config.sprites.Count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemPSD>());
                }
                items[i].Refresh(config.sprites[i]);
            }
            for (int i = config.sprites.Count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }
    }
}