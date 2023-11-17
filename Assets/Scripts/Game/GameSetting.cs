using Data;
using Game.Resource;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class GameSetting
    {
        [SerializeField] private LoadingType _loading;

        [SerializeField] private bool _sleep = false;

        [SerializeField] private bool _background = true;

        [SerializeField] private bool _vSync = false;

        [SerializeField] private int _frameRate = 60;

        [SerializeField] private float _timeScale = 1f;

        public void Initialize()
        {
            Loading = _loading;

            Sleep = _sleep;

            RunBackground = _background;

            vSyncCount = _vSync;

            FrameRate = _frameRate;

            TimeScale = _timeScale;
        }

        public LoadingType Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;

                ResourceConfig.Loading = value;
            }
        }

        public bool Sleep
        {
            get
            {
                return _sleep;
            }
            set
            {
                _sleep = value;

                Screen.sleepTimeout = _sleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        public bool RunBackground
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;

                Application.runInBackground = _background;
            }
        }

        public bool vSyncCount
        {
            get
            {
                return _vSync;
            }
            set
            {
                _vSync = value;

                QualitySettings.vSyncCount = _vSync ? 0 : 1;
            }
        }

        public int FrameRate
        {
            get
            {
                return _frameRate;
            }
            set
            {
                _frameRate = value;

                Application.targetFrameRate = _frameRate;
            }
        }

        public float TimeScale
        {
            get
            {
                return _timeScale;
            }
            set
            {
                _timeScale = value;

                Time.timeScale = _timeScale;
            }
        }

        public string Version
        {
            get
            {
                return Application.version;
            }
        }
    }
}