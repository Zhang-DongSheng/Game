using Game.Resource;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private void Awake()
        {
            LoginLogic.Instance.Init();

            WarehouseLogic.Instance.Init();

            ShopLogic.Instance.Init();

            TaskLogic.Instance.Init();

            ActivityLogic.Instance.Init();

            ReddotLogic.Instance.Init();
#if HOTFIX
            ILRuntimeLogic.Instance.Init();
#endif
            ResourceManager.Initialize(GameConfig.Load);

            ScheduleLogic.Instance.callback = () =>
            {
                ResourceManager.UpdateDependencies();

                UIQuickEntry.OpenSingle(UIPanel.UILogin);
            };
            ScheduleLogic.Instance.Init();
        }

        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Application.runInBackground = true;

            Application.targetFrameRate = 60;
        }
    }
}