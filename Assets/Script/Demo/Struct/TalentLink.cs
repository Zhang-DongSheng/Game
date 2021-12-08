using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class TalentLink : Talent
    {
        public TalentDirection direction;

        public Vector2Int skills;

        [HideInInspector] public float angle;

        [HideInInspector] public int src, dst;

        public Vector2 Init(TalentSkill talent)
        {
            TalentUtils.Link(direction, ref src, ref dst);

            src += skills.x * 10;

            dst += skills.y * 10;

            Vector2 position = TalentUtils.Position(talent.position, direction);

            base.position = Vector2.Lerp(talent.position, position, 0.5f);

            angle = TalentUtils.Rotation(direction);

            return position;
        }
    }
}