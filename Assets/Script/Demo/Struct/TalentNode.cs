using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TalentNode : Talent
    {
        public int index;

        [HideInInspector] public int front, back;

        public void Init(int index)
        {
            this.index = index;

            front = index < 5 ? index + 1 : 0;

            back = index > 0 ? index - 1 : 5;

            position = TalentUtils.Position(index);
        }
    }
}