using Game.Model;
using Game.UI;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class GameController : MonoSingleton<GameController>
    {
        public ModelDisplay model;

        private void Awake()
        {
            ScheduleLogic.Instance.Init();

            UserLogic.Instance.Init();

            BagLogic.Instance.Init();

            ShopLogic.Instance.Init();

            ActivityLogic.Instance.Init();

            ReddotLogic.Instance.Init();
#if DEBUG
            ILRuntimeLogic.Instance.Init();
#endif
        }

        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Application.runInBackground = true;

            Application.targetFrameRate = 60;

            if (Application.isPlaying)
            {
                UIManager.Instance.Open(UIPanel.UILogin);
            }

            if (model != null)
            {
                model.Hide();
            }
        }

        private void Update()
        {
            
        }
    }
}