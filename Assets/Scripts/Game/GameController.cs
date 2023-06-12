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
#if HOTFIX
            ILRuntimeLogic.Instance.Initialize();
#endif
            DevelopLogic.Instance.Initialize();

            ResourceManager.Initialize(ResourceConfig.Loading);

            GameStateController.Instance.Init();

            yield return new WaitForSeconds(3);

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLoginState>();
            };
            ScheduleLogic.Instance.Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                DevelopLogic.Instance.Break();

                Debuger.Log(Author.File, "日志保存成功");
            }
        }
    }
}