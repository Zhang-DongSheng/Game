using Game.Model;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {
        public ModelDisplay model;

        private void Awake()
        {
            ScheduleLogic.Instance.Init();

            UserLogic.Instance.Init();

            BagLogic.Instance.Init();

            ShopLogic.Instance.Init();

            TaskLogic.Instance.Init();

            ActivityLogic.Instance.Init();

            ReddotLogic.Instance.Init();
#if HOTFIX
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
                UIManager.Instance.Open(UI.UIPanel.UILogin);
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