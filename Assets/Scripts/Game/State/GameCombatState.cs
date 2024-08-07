﻿using Game.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game.State
{
    /// <summary>
    /// 游戏战斗
    /// </summary>
    public class GameCombatState : IGameState
    {
        protected readonly int sceneIndex = 3;

        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UIManager.Instance.CloseAll(true);

            LoadingView.Instance.Open();

            var scene = SceneManager.GetActiveScene();

            if (scene != null && scene.buildIndex == sceneIndex)
            {
                LoadUI();
            }
            else
            {
                RuntimeManager.Instance.StartCoroutine(LoadSceneAsync(sceneIndex));
            }
        }

        public void OnStay()
        {

        }

        public void OnExit()
        {

        }

        private IEnumerator LoadSceneAsync(int index)
        {
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            LoadUI();
        }

        private void LoadUI()
        {
            UIQuickEntry.OpenSingle(UIPanel.Battle, callback: LoadingView.Instance.Close);
        }
    }
}