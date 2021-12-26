using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TalentSkill : Talent
    {
        public List<TalentNode> children;

        public TalentSkill()
        {
            children = new List<TalentNode>();

            for (int i = 0; i < TalentConfig.CHILDREN; i++)
            {
                children.Add(new TalentNode());
            }
        }

        public void Init()
        {
            int last = TalentConfig.CHILDREN - 1;

            for (int i = 0; i < TalentConfig.CHILDREN; i++)
            {
                if (i == 0)
                {
                    children[i].neighbours = new Vector3Int(children[last].ID, children[1].ID, 0);
                }
                else if (i == last)
                {
                    children[i].neighbours = new Vector3Int(children[i - 1].ID, children[0].ID, 0);
                }
                else
                {
                    children[i].neighbours = new Vector3Int(children[i - 1].ID, children[i + 1].ID, 0);
                }
            }
        }

        public void Update(int index, int talentID)
        {
            if (children.Count > index)
            {
                children[index].neighbours.z = talentID;
            }
        }
    }
}