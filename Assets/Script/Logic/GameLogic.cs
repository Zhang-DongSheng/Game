using Data;
using Game.Model;
using Game.UI;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class GameLogic : MonoSingleton<GameLogic>
    {
        public ModelDisplay model;

        public DataLanguage Language { get; private set; }

        private void Awake()
        {
            Language = DataManager.Instance.Load<DataLanguage>("language", "Data/Language");

            Language.Init();
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                model.Hide();

                UIManager.Instance.Open(UIKey.UILogin);
            }
        }
    }
}