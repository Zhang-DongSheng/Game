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
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                model.Hide();

                UIManager.Instance.Open(UIPanel.UILogin);
            }
        }

        private void Update()
        {
            
        }
    }
}