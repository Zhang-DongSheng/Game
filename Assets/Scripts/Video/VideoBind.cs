using Game.Resource;
using System.IO;
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

            if (m_video.playOnAwake)
            {
                Play();
            }
        }
        [ContextMenu("Play")]
        public void Play()
        {
            if (string.IsNullOrEmpty(url)) return;

            string video = Path.GetFileNameWithoutExtension(url);

            if (m_video.clip != null && m_video.clip.name == video)
            {
                if (m_video.isPlaying == false)
                {
                    m_video.Play();
                }
            }
            else
            {
                if (m_video.isPlaying)
                {
                    m_video.Stop();
                }
                ResourceManager.LoadAsync<VideoClip>(url, (clip) =>
                {
                    m_video.clip = clip;

                    m_video.Play();
                });
            }
        }
        [ContextMenu("Pause")]
        public void Pause()
        {
            if (m_video.isPaused)
            {
                m_video.Play();
            }
            else
            {
                m_video.Pause();
            }
        }
        [ContextMenu("Stop")]
        public void Stop()
        {
            if (m_video.isPlaying)
            {
                m_video.Stop();
            }
            else if (m_video.isPaused)
            {
                m_video.Stop();
            }
        }
    }
}