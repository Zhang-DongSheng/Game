using Game.Resource;
using Game.State;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Application.runInBackground = true;

            QualitySettings.vSyncCount = 0;

            Application.targetFrameRate = 60;
        }

        private void Start()
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
            ResourceManager.Initialize(ResourceConfig.Loading);

            GameStateController.Instance.Init();

            // 这边等3s闪屏然后进入Loading界面...

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLoginState>();
            };
            ScheduleLogic.Instance.Init();
        }
    }
}