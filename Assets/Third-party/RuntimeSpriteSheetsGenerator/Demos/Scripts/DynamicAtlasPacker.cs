using DaVikingCode.RectanglePacking;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace UnityEngine
{
    public class DynamicAtlasPacker
    {
        private readonly int width = 2048;

        private readonly int height = 2048;

        private readonly int padding = 8;

        private readonly Dictionary<string, List<DynamicAtlas>> m_atlas = new Dictionary<string, List<DynamicAtlas>>();

        public Sprite Pop(string atlas, string key)
        {
            if (string.IsNullOrEmpty(atlas)) return null;

            if (m_atlas.ContainsKey(atlas))
            {
                for (int i = 0; i < m_atlas[atlas].Count; i++)
                {
                    if (m_atlas[atlas][i].Exists(key))
                    {
                        return m_atlas[atlas][i].Pop(key);
                    }
                }
            }
            return null;
        }

        public void Push(string atlas, string key, Texture2D texture, Action<bool> callBack = null)
        {
            if (string.IsNullOrEmpty(atlas)) return;

            if (texture.width < width && texture.height < height)
            {
                if (m_atlas.ContainsKey(atlas))
                {
                    if (m_atlas[atlas].Exists(x => x.Exists(key)))
                    {
                        Debug.LogWarning("The texture is exist!, Texture name is " + key);
                    }
                    else
                    {
                        bool done = false;

                        for (int i = 0; i < m_atlas[atlas].Count; i++)
                        {
                            if (!m_atlas[atlas][i].Enough)
                            {
                                if (m_atlas[atlas][i].Push(key, texture))
                                {
                                    done = true;
                                    break;
                                }
                            }
                        }

                        if (!done)
                        {
                            m_atlas[atlas].Add(new DynamicAtlas(atlas, width, height, padding));
                            m_atlas[atlas][m_atlas.Count - 1].Push(key, texture);
                        }
                    }
                }
                else
                {
                    m_atlas.Add(atlas, new List<DynamicAtlas>() { new DynamicAtlas(atlas, width, height, padding) });
                    m_atlas[atlas][0].Push(key, texture);
                }
                callBack?.Invoke(true);
            }
            else
            {
                callBack?.Invoke(false);
            }
        }
    }

    public class DynamicAtlas
    {
        private readonly string atlas;

        private readonly bool compress;

        private readonly int width, height, padding;

        private bool enough;

        private readonly Color32[] m_color;

        private readonly RectanglePacker m_packer;

        private readonly List<DynamicSprite> m_sprites = new List<DynamicSprite>();

        private Texture2D m_texture;

        public DynamicAtlas(string atlas, int width, int height, int padding, bool compress = true)
        {
            this.atlas = atlas;

            this.width = width;

            this.height = height;

            this.padding = padding;

            this.compress = compress;

            m_packer = new RectanglePacker(width, height, padding);

            m_texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            m_color = m_texture.GetPixels32();
        }

        public bool Push(string key, Texture2D texture)
        {
            if (string.IsNullOrEmpty(key)) return true;

            if (texture == null) return true;

            for (int i = 0; i < m_sprites.Count; i++)
            {
                m_sprites[i].index = i;
                m_sprites[i].color = m_texture.GetPixels((int)m_sprites[i].rect.x, (int)m_sprites[i].rect.y, (int)m_sprites[i].rect.width, (int)m_sprites[i].rect.height);
            }

            m_sprites.Add(new DynamicSprite()
            {
                key = key,
                index = -1,
                rect = new Rect(0, 0, texture.width, texture.height),
                color = texture.GetPixels(),
            });

            m_packer.reset(width, height, padding);

            for (int i = 0; i < m_sprites.Count; i++)
            {
                m_packer.insertRectangle((int)m_sprites[i].rect.width, (int)m_sprites[i].rect.height, m_sprites[i].index);
            }

            int count = m_packer.packRectangles();

            enough = m_sprites.Count > count;

            if (enough)
            {
                m_sprites.RemoveAt(m_sprites.FindIndex(x => x.key == key));

                for (int i = 0; i < m_sprites.Count; i++)
                {
                    m_sprites[i].color = null;
                }

                return false;
            }
            else
            {
                if (texture != null) { texture = null; }

                if (m_texture.format != TextureFormat.RGBA32)
                {
                    Object.Destroy(m_texture);

                    m_texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                }
                m_texture.SetPixels32(m_color);

                IntegerRectangle rect = new IntegerRectangle();

                Color32[] color;

                int index, id, size;

                for (int i = 0; i < count; i++)
                {
                    rect = m_packer.getRectangle(i, rect);

                    id = m_packer.getRectangleId(i);

                    index = m_sprites.FindIndex(x => x.index == id);

                    if (index != -1)
                    {
                        m_sprites[index].rect = new Rect(rect.x, rect.y, rect.width, rect.height);

                        color = new Color32[m_sprites[index].color.Length];

                        for (int j = 0; j < m_sprites[index].color.Length; j++)
                        {
                            color[j] = m_sprites[index].color[j];
                        }
                    }
                    else
                    {
                        size = rect.width * rect.height;

                        color = new Color32[size];

                        for (int j = 0; j < size; j++)
                        {
                            color[j] = Color.black;
                        }
                    }
                    m_texture.SetPixels32(rect.x, rect.y, rect.width, rect.height, color);
                }

                for (int i = 0; i < m_sprites.Count; i++)
                {
                    m_sprites[i].color = null;
                }

                #region UNITY_EDITOR
                if (false)
                {
                    File.WriteAllBytes(Application.dataPath + string.Format("/{0}.png", atlas), m_texture.EncodeToPNG());

                    AssetDatabase.Refresh();
                }
                #endregion

                if (compress)
                {
                    m_texture.Compress(true);
                }

                m_texture.Apply();

                return true;
            }
        }

        public Sprite Pop(string key)
        {
            int index = m_sprites.FindIndex(x => x.key == key);

            if (index != -1 && index < m_sprites.Count)
            {
                return Sprite.Create(m_texture, m_sprites[index].rect, Vector2.one * 0.5f);
            }
            return null;
        }

        public bool Exists(string key)
        {
            return m_sprites.Exists(x => x.key == key);
        }

        public bool Enough { get { return enough; } }
    }

    public class DynamicSprite
    {
        public string key;

        public int index;

        public Rect rect;

        public Color[] color;
    }
}