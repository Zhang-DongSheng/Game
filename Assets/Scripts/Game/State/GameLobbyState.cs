using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.State
{
    /// <summary>
    /// 游戏大厅
    /// </summary>
    public class GameLobbyState : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            RuntimeManager.Instance.StartCoroutine(LoadSceneAsync(1));
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

            UIQuickEntry.OpenSingle(UIPanel.UIMain);
        }
    }
}
