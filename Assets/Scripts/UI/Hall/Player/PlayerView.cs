using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class PlayerView : ViewBase
    {
        private readonly string character = "Package/Prefab/Model/Character/Female.prefab";

        [SerializeField] private ItemStep hair;

        [SerializeField] private ItemStep face;

        [SerializeField] private ItemStep clothes;

        [SerializeField] private ItemStep pants;

        [SerializeField] private ItemStep shoes;

        protected override void OnAwake()
        {
            hair.onValueChanged.AddListener(OnClickHair);

            face.onValueChanged.AddListener(OnClickFace);

            clothes.onValueChanged.AddListener(OnClickClothes);

            pants.onValueChanged.AddListener(OnClickPants);

            shoes.onValueChanged.AddListener(OnClickShoes);
        }

        public override void Refresh(UIParameter parameter)
        {
            hair.Initialize(new List<int>() { 1, 2 });

            face.Initialize(new List<int>() { 1, 2 });

            clothes.Initialize(new List<int>() { 1, 2 });

            pants.Initialize(new List<int>() { 1, 2 });

            shoes.Initialize(new List<int>() { 1, 2 });
        }

        private void OnClickHair(int index)
        {
            //ModelManager.Instance.Modify(character, "hair", index.ToString(), "blue");
        }

        private void OnClickFace(int index)
        {
            //ModelManager.Instance.Modify(character, "face", index.ToString(), "blue");
        }

        private void OnClickClothes(int index)
        {
            //ModelManager.Instance.Modify(character, "top", index.ToString(), "blue");
        }

        private void OnClickPants(int index)
        {
            //ModelManager.Instance.Modify(character, "pants", index.ToString(), "blue");
        }

        private void OnClickShoes(int index)
        {
            //ModelManager.Instance.Modify(character, "shoes", index.ToString(), "blue");
        }

        private readonly Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> _parts = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();

        public void Initialize(Transform target)
        {
            _parts.Clear();

            var parts = target.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (var part in parts)
            {
                string[] split = part.name.Split('-');

                string key = split[0];

                string index = split.Length > 1 ? split[1] : "-1";

                if (_parts.ContainsKey(key))
                {
                    _parts[key].Add(index, part);
                }
                else
                {
                    _parts.Add(key, new Dictionary<string, SkinnedMeshRenderer>() { { index, part } });
                }
            }
        }

        public void Change(string part, string index, string skin)
        {
            foreach (var parts in _parts)
            {
                if (parts.Key == part)
                {
                    foreach (var p in parts.Value)
                    {
                        bool active = p.Key == index;

                        if (active)
                        {

                        }
                        SetActive(p.Value, active);
                    }
                    break;
                }
            }
        }
    }
}