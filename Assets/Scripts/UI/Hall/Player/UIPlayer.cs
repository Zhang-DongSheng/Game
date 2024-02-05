using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIPlayer : UIBase
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
            ModelManager.Instance.Modify(character, "hair", index.ToString(), "blue");
        }

        private void OnClickFace(int index)
        {
            ModelManager.Instance.Modify(character, "face", index.ToString(), "blue");
        }

        private void OnClickClothes(int index)
        {
            ModelManager.Instance.Modify(character, "top", index.ToString(), "blue");
        }

        private void OnClickPants(int index)
        {
            ModelManager.Instance.Modify(character, "pants", index.ToString(), "blue");
        }

        private void OnClickShoes(int index)
        {
            ModelManager.Instance.Modify(character, "shoes", index.ToString(), "blue");
        }
    }
}