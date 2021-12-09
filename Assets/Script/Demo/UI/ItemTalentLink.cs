using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemTalentLink : MonoBehaviour
    {
        [SerializeField] private RectTransform self;

        [SerializeField] private ItemTalentNode item;

        [SerializeField] private ItemTalentLine front;

        [SerializeField] private ItemTalentLine back;

        private TalentLink talent;

        public void Initialize(TalentLink talent)
        {
            this.talent = talent;

            item.Initialize(talent);

            front.Initialize(talent.neighbours.x, talent.ID);

            back.Initialize(talent.ID, talent.neighbours.y);

            self.anchoredPosition = talent.position;

            self.eulerAngles = new Vector3(0, 0, talent.angle);
        }

        public void Refresh(List<int> list)
        {
            item.Refresh(TalentUtils.Status(list, talent.ID));

            front.Refresh(list);

            back.Refresh(list);
        }
    }
}
