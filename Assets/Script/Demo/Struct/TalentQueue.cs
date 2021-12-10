using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class TalentQueue
    {
        public int key;

        public List<TalentPossible> possibles = new List<TalentPossible>();
    }
    [System.Serializable]
    public class TalentPossible
    {
        public int final;

        public List<int> routes = new List<int>();

        public int Index(int node)
        {
            int index = -1, count = routes.Count;

            for (int i = 0; i < count; i++)
            {
                if (routes[i] == node)
                {
                    index = i;
                }
            }
            return index;
        }
    }
}