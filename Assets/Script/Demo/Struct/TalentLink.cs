using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TalentLink : Talent
    {
        public TalentDirection direction;

        public Vector2Int skills;

        [HideInInspector] public float angle;

        public void Init(TalentSkill src, TalentSkill dst)
        {
            neighbours = TalentUtils.Neighbours(direction, src, dst);

            Vector2 position = TalentUtils.Position(src.position, direction);

            base.position = Vector2.Lerp(src.position, position, 0.5f);

            angle = TalentUtils.Rotation(direction);

            dst.position = position;
        }
    }
}