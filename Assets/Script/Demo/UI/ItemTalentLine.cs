using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemTalentLine : MonoBehaviour
    {
        [SerializeField] private ItemTalentStatus status;

        private Vector2Int nodes;

        public void Initialize(int from, int to)
        {
            nodes = new Vector2Int(from, to);
        }

        public void Refresh(List<int> list)
        {
            this.status.Refresh(TalentUtils.Status(list, nodes.x, nodes.y));
        }
    }
}
