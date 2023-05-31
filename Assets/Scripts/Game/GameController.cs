using Game.Resource;
using Game.State;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
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
            StartCoroutine(Initialize());
        }

        private new IEnumerator Initialize()
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

            yield return new WaitForSeconds(3);

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLoginState>();
            };
            ScheduleLogic.Instance.Init();
        }
    }
}