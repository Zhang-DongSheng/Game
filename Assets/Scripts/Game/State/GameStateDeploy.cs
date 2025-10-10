using Game.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game.State
{
    public class GameStateDeploy : IGameState
    {
        protected readonly int sceneIndex = 2;

        public void OnCreate()
        {

        }

        public void OnEnter()
        {
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
            UIQuickEntry.Open(UIPanel.Deploy, callback: LoadingView.Instance.Close);
        }
    }
}