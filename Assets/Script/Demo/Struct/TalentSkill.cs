using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class TalentSkill : Talent
    {
        public bool root;

        public List<TalentNode> children;

        public TalentSkill()
        {
            children = new List<TalentNode>();

            for (int i = 0; i < TalentConfig.CHILDREN; i++)
            {
                children.Add(new TalentNode());
            }
        }
    }
}