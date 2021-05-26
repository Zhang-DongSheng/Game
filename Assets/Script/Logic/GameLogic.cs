using Game.Model;
using Game.UI;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class GameLogic : MonoSingleton<GameLogic>
    {
        public ModelDisplay model;

        private void Awake()
        {

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