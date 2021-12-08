using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "talent.asset", menuName = "Talent")]
    public class TalentSystem : ScriptableObject
    {
        public TalentSkill trunk;

        public List<TalentSkill> branches;

        public List<TalentLink> links;

        private void OnValidate()
        {
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i] != null && branches[i].children.Count != TalentConfig.CHILDREN)
                {
                    branches[i] = new TalentSkill();
                }
            }
        }

        [ContextMenu("Skill ID")]
        protected void Format()
        {
            trunk.ID = 101;

            for (int i = 0; i < trunk.children.Count; i++)
            {
                trunk.children[i].ID = trunk.ID * 10 + i;
            }

            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i] != null)
                {
                    branches[i].ID = 102 + i;

                    for (int j = 0; j < branches[i].children.Count; j++)
                    {
                        branches[i].children[j].ID = branches[i].ID * 10 + j;
                    }
                }
            }
        }

        [ContextMenu("Link")]
        protected void Link()
        {
            for (int i = 0; i < links.Count; i++)
            {
                TalentSkill root = links[i].skills.x == trunk.ID ? trunk : branches.Find(x => x.ID == links[i].skills.x);

                for (int j = 0; j < branches.Count; j++)
                {
                    if (branches[j].ID == links[i].skills.y)
                    {
                        branches[j].position = links[i].Init(root);
                        break;
                    }
                }
            }
        }
    }
}