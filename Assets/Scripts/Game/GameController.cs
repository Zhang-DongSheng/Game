using Game.Resource;
using Game.State;
using UnityEngine;
using System.Collections;
using Game.UI;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {
        [SerializeField] private GameSetting setting;

        private void Awake()
        {
            setting.Initialize();
        }

        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private new IEnumerator Initialize()
        {
            LoginLogic.Instance.Initialize();

            SettingLogic.Instance.Initialize();

            PlayerLogic.Instance.Initialize();

            WarehouseLogic.Instance.Initialize();

            TaskLogic.Instance.Initialize();

            ShopLogic.Instance.Initialize();

            ActivityLogic.Instance.Initialize();

            NotificationLogic.Instance.Initialize();

            ReddotLogic.Instance.Initialize();

            IFixLogic.Instance.Initialize();

            ILRuntimeLogic.Instance.Initialize();

            ResourceManager.Initialize(setting.Loading);

            GameStateController.Instance.Init();

            yield return new WaitForSeconds(3);

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLoginState>();
            };
            ScheduleLogic.Instance.Initialize();
        }
    }
}