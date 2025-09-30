using Game.Logic;
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

        [SerializeField] private float seconds;

        private YieldInstruction wait;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            wait = new WaitForSeconds(seconds);

            StartCoroutine(Laoding());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                UIQuickEntry.Open(UIPanel.Console);
            }
        }

        private IEnumerator Laoding()
        {
            CameraController.Instance.Initialize();

            UIManager.Instance.Initialize();

            HotfixLogic.Instance.Initialize();

            SettingLogic.Instance.Initialize();

            NotificationLogic.Instance.Initialize();

            NetworkMessageManager.Instance.Initialize();

            LoginLogic.Instance.Initialize();

            PlayerLogic.Instance.Initialize();

            ChatLogic.Instance.Initialize();

            ClubLogic.Instance.Initialize();

            FriendLogic.Instance.Initialize();

            ReddotLogic.Instance.Initialize();

            PopupLogic.Instance.Initialize();

            DialogSystemLogic.Instance.Initialize();

            WarehouseLogic.Instance.Initialize();

            MailLogic.Instance.Initialize();

            ActivityLogic.Instance.Initialize();

            ShopLogic.Instance.Initialize();

            RankingListLogic.Instance.Initialize();

            TaskLogic.Instance.Initialize();

            GuidanceLogic.Instance.Initialize();

            ConsoleLogic.Instance.Initialize();

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

            yield return wait;

            Network.NetworkManager.Instance.Connection();

            ScheduleLogic.Instance.callback = () =>
            {
                GameStateController.Instance.EnterState<GameLoginState>();
            };
            ScheduleLogic.Instance.Initialize();
        }

        private void OnRelease()
        {
            Network.NetworkManager.Instance.Close();

            HotfixLogic.Instance.Release();

            ResourceManager.UnLoadAllAsset();
        }

        private void OnApplicationPause(bool pause)
        {
            Debuger.LogWarning(Author.Device, "应用程序切换后台" + pause);
        }

        private void OnApplicationQuit()
        {
            OnRelease();

            Debuger.LogWarning(Author.Device, "应用程序退出");
        }
    }
}