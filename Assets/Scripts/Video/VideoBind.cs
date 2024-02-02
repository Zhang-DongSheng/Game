using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Game
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoBind : MonoBehaviour
    {
        [SerializeField] private VideoPlayer m_video;

        [SerializeField] private RawImage screen;

        [SerializeField] private Vector2Int size;

        [SerializeField] private string url;

        private RenderTexture texture;

        private void Awake()
        {
            Relevance();
        }

        private void Relevance()
        {
            if (texture == null)
            {
                texture = new RenderTexture(size.x, size.y, 1);
            }
            screen.texture = texture;

            m_video.targetTexture = texture;
        }
        [ContextMenu("Play")]
        public void Play()
        {
            m_video.Play();
        }
        [ContextMenu("Stop")]
        public void Stop()
        {
            m_video.Stop();
        }
    }
}
