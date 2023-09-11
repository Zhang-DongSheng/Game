using Game.Resource;
using Game.State;
using UnityEngine;
using System.Collections;

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

            WarehouseLogic.Instance.Initialize();

            ShopLogic.Instance.Initialize();

            TaskLogic.Instance.Initialize();

            ActivityLogic.Instance.Initialize();

            ReddotLogic.Instance.Initialize();

            ILRuntimeLogic.Instance.Initialize();

            ResourceManager.Initialize(ResourceConfig.Loading);

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