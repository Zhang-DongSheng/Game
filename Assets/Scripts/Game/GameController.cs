using Game.Resource;
using Game.State;
using Game.UI;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {
        [SerializeField] private GameMode mode;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            StartCoroutine(Laoding());
        }

        private IEnumerator Laoding()
        {
            UIManager.Instance.Initialize();

            LoginLogic.Instance.Initialize();

            PlayerLogic.Instance.Initialize();

            WarehouseLogic.Instance.Initialize();

            TaskLogic.Instance.Initialize();

            ShopLogic.Instance.Initialize();

            MailLogic.Instance.Initialize();

            ActivityLogic.Instance.Initialize();

            NotificationLogic.Instance.Initialize();

            SettingLogic.Instance.Initialize();

            ReddotLogic.Instance.Initialize();

            GuidanceLogic.Instance.Initialize();

            switch (mode)
            {
                case GameMode.Develop:
                    ResourceManager.Initialize(LoadingType.AssetDatabase);
                    break;
                default:
                    ResourceManager.Initialize(LoadingType.AssetBundle);
                    break;
            }
            GameStateController.Instance.Init();

            yield return new WaitForSeconds(3);

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLoginState>();
            };
            ScheduleLogic.Instance.Initialize();
        }

        private void OnApplicationPause(bool pause)
        {
            Debuger.LogError(Author.Script, "应用程序切换后台" + pause);
        }

        private void OnApplicationQuit()
        {
            Debuger.LogError(Author.Script, "应用程序退出");
        }

        private void OnDestroy()
        {
            ResourceManager.UnLoadAllAsset();
        }
    }
}