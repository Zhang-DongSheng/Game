using Game.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game.State
{
    public class GameDeployState : IGameState
    {
        protected readonly int sceneIndex = 2;

        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UILoading.Instance.Open();

            var scene = SceneManager.GetActiveScene();

            if (scene != null && scene.buildIndex == sceneIndex)
            {
                UIQuickEntry.Open(UIPanel.UIDeploy);
            }
            else
            {
                RuntimeManager.Instance.StartCoroutine(LoadSceneAsync(sceneIndex));
            }
        }

        public void OnExit()
        {
            
        }

        public void OnStay()
        {

        }

        private IEnumerator LoadSceneAsync(int index)
        {
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            UIManager.Instance.Open(UIPanel.UIDeploy);
        }
    }
}