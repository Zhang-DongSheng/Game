using Game.State;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIDeploy : UIBase
    {
        [SerializeField] private ItemDeployForDrag current;

        [SerializeField] private PrefabTemplateBehaviour prefab;

        private readonly List<ItemDeploy> deploys = new List<ItemDeploy>();

        public override void Refresh(UIParameter parameter)
        {
            int count = 10;

            for (int i = 0; i < count; i++)
            {
                if (i >= deploys.Count)
                {
                    var item = prefab.Create<ItemDeploy>();
                    item.callback = OnDrag;
                    deploys.Add(item);
                }
                deploys[i].Refresh(i);
            }
        }

        private void OnDrag(int index, Vector3 position)
        {
            current.transform.position = position;

            current.Refresh(index);
        }

        public override bool Back()
        {
            OnClickClose(); return false;
        }

        protected override void OnClickClose()
        {
            DeployLogic.Instance.Dispose();

            GameStateController.Instance.EnterState<GameLobbyState>();
        }
    }
}