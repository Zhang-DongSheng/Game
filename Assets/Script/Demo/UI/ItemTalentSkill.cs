using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemTalentSkill : MonoBehaviour
    {
        [SerializeField] private RectTransform self;

        [SerializeField] private ItemTalentNode skill;

        [SerializeField] private List<ItemTalentNode> items;

        [SerializeField] private List<ItemTalentLine> lines;

        private TalentSkill talent;

        public void Initialize(TalentSkill talent, Action<int> callback)
        {
            this.talent = talent;

            this.skill.Initialize(talent, OnClickSkill);

            int count = Mathf.Min(items.Count, talent.children.Count);

            for (int i = 0; i < count; i++)
            {
                if (items[i] != null)
                {
                    items[i].Initialize(talent.children[i], callback);
                }
            }

            count = lines.Count - 1;

            for (int i = 0; i < count; i++)
            {
                if (lines[i] != null)
                {
                    lines[i].Initialize(talent.children[i].ID, talent.children[i + 1].ID);
                }
            }
            lines[count].Initialize(talent.children[count].ID, talent.children[0].ID);

            self.anchoredPosition = talent.position;
        }

        public void Refresh(List<int> list, List<int> preview)
        {
            skill.Refresh(TalentUtils.Status(list, preview, talent.ID));

            int count = Mathf.Min(items.Count, talent.children.Count);

            for (int i = 0; i < count; i++)
            {
                if (items[i] != null)
                {
                    items[i].Refresh(TalentUtils.Status(list, preview, talent.children[i].ID));
                }
            }

            count = lines.Count;

            for (int i = 0; i < count; i++)
            {
                if (lines[i] != null)
                {
                    lines[i].Refresh(list, preview);
                }
            }
        }

        private void OnClickSkill(int skillID)
        {
            //ÌØÊâ´¦Àí
        }
    }
}