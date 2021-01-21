using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Game
{
    [ExecuteInEditMode]
    public class GameLogic : MonoSingleton<GameLogic>
    {
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
                UIManager.Instance.Open(UIKey.UILogin);
            }
        }
    }
}