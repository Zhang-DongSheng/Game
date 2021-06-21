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