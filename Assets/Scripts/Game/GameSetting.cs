using Data;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class GameSetting
    {
        [SerializeField] private GameMode _mode;

        [SerializeField] private Language _language;

        [SerializeField] private bool _sleep = false;

        [SerializeField] private bool _background = true;

        [SerializeField] private bool _vSync = false;

        [SerializeField] private int _frameRate = 60;

        [SerializeField] private float _timeScale = 1f;

        public void Initialize()
        {
            GameMode = _mode;

            Language = _language;

            Sleep = _sleep;

            RunBackground = _background;

            vSyncCount = _vSync;

            FrameRate = _frameRate;

            TimeScale = _timeScale;
        }

        public GameMode GameMode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        public Language Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
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