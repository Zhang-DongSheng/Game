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
        public List<int> routes = new List<int>();
    }
}