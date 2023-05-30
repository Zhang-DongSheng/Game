using Game.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game.State
{
    /// <summary>
    /// 游戏战斗
    /// </summary>
    public class GameCombatState : IGameState
    {
        protected readonly int sceneIndex = 2;

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
                UIQuickEntry.OpenSingle(UIPanel.UIMMORPG);
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

            UIManager.Instance.Open(UIPanel.UIMMORPG);
        }
    }
}
