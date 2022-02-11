using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    public class DataPSD : DataBase
    {
        public List<PSDInformation> list = new List<PSDInformation>();

        public PSDInformation this[int index]
        {
            get
            {
                if (list.Count > index)
                {
                    return list[index];
                }
                return null;
            }
        }

        public PSDInformation Find(string index)
        {
            return list.Find(x => x.name == index);
        }
        [ContextMenu("Create")]
        internal void Create()
        {
            PSDInformation info = this[0];

            Canvas canvas = FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
            }
            RectTransform parent = new GameObject(info.name).AddComponent<RectTransform>();

            parent.SetParent(canvas.transform);

            for (int i = 0; i < info.sprites.Count; i++)
            {
                if (info.sprites[i] != null)
                {
                    GameObject go = new GameObject(info.sprites[i].name);
                    go.transform.SetParent(parent);
                    RectTransform rect = go.AddComponent<RectTransform>();
                    rect.anchoredPosition = info.sprites[i].position;
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, info.sprites[i].size.x);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, info.sprites[i].size.y);
                    Image image = go.AddComponent<Image>();
                    image.sprite = info.sprites[i].sprite;
                }
            }
        }
    }
    [Serializable]
    public class PSDInformation
    {
        public string name;

        public List<SpriteInformation> sprites = new List<SpriteInformation>();

        public Dictionary<string, List<SpriteInformation>> groups = new Dictionary<string, List<SpriteInformation>>();

        public string description;
    }
    [Serializable]
    public class SpriteInformation
    {
        public string name;

        public Sprite sprite;

        public Vector2 position;

        public Vector2 size;

        public int order;

        public string description;
    }
}