using Game.UI;
using System.Collections;
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
            UIManager.Instance.CloseAll(true);

            UILoading.Instance.Open();

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

        public void OnExit()
        {
            ModelLogic.Instance.Clear();
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
            UIQuickEntry.OpenSingle(UIPanel.UIMain, callback: UILoading.Instance.Close);
        }
    }
}