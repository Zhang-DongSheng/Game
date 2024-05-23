using Game.Model;
using Game.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.State
{
    /// <summary>
    /// 游戏大厅
    /// </summary>
    public class GameLobbyState : IGameState
    {
        protected readonly int sceneIndex = 1;

        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            Debuger.Log(Author.Script, "进入大厅");

            //UIManager.Instance.CloseAll(true);

            //UILoading.Instance.Open();

            var scene = SceneManager.GetActiveScene();

            if (scene != null && scene.buildIndex == sceneIndex)
            {

                Debuger.Log(Author.Script, "进入大厅3");


                LoadUI();
            }
            else
            {

                Debuger.Log(Author.Script, "进入大厅2");


                RuntimeManager.Instance.StartCoroutine(LoadSceneAsync(sceneIndex));
            }
        }

        public void OnExit()
        {
            ModelDisplayManager.Instance.Clear();
        }

        public void OnStay()
        {

        }

        private IEnumerator LoadSceneAsync(int index)
        {
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            LoadUI();
        }

        private void LoadUI()
        {
            UIQuickEntry.OpenSingle(UIPanel.Main, callback: UILoading.Instance.Close);
        }
    }
}