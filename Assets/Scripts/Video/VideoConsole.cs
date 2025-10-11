using Game.Attribute;
using Game.Resource;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Game
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoConsole : MonoBehaviour
    {
        [FieldName("播放器")]
        [SerializeField] private VideoPlayer player;
        [FieldName("屏幕")]
        [SerializeField] private RawImage screen;
        [FieldName("分辨率")]
        [SerializeField] private Vector2Int size;
        [FieldName("地址")]
        [SerializeField] private string url;

        private RenderTexture texture;

        private void Awake()
        {
            texture = RenderTexture.GetTemporary(size.x, size.y, 1);

            player.targetTexture = texture;

            screen.texture = texture;

            Switch();
        }

        public void Switch()
        {
            if (string.IsNullOrEmpty(url)) return;

            ResourceManager.LoadAsync<VideoClip>(url, (clip) =>
            {
                player.clip = clip;

                if (player.playOnAwake)
                {
                    Play();
                }
            });
        }

        public void Play()
        {
            if (player.clip != null)
            {
                if (player.isPlaying == false)
                {
                    player.Play();
                }
            }
            else
            {
                Debuger.LogError(Author.UI, $"VideoClip is Null!");
            }
        }

        public void Pause()
        {
            if (player.isPaused)
            {
                player.Play();
            }
            else
            {
                player.Pause();
            }
        }

        public void Stop()
        {
            if (player.isPlaying)
            {
                player.Stop();
            }
            else if (player.isPaused)
            {
                player.Stop();
            }
        }

        private void OnDestroy()
        {
            if (texture != null)
            {
                RenderTexture.ReleaseTemporary(texture);
            }
            texture = null;
        }
    }
}