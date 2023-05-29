using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    [DisallowMultipleComponent]
    public class ModelScale : ItemBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private List<ModelScaleChild> children;

        [SerializeField] private List<ModelScaleParticle> particles;

        [SerializeField] private ModelScaleType type;

        private Vector3 scale = Vector3.one;

        public void SetScale(float size)
        {
            scale.x = size;

            scale.y = size;

            scale.z = size;

            SetScale();
        }

        public void SetScale(float x, float y)
        {
            scale.x = x;

            scale.y = 1;

            scale.z = y;

            SetScale();
        }

        public void SetScale(float x, float y, float z)
        {
            scale.x = x;

            scale.y = y;

            scale.z = z;

            SetScale();
        }

        public void SetScale()
        {
            if (type == ModelScaleType.None) return;
            // 主节点
            if ((type & ModelScaleType.Self) != 0)
            {
                target.localScale = scale;
            }
            // 子节点单独修改
            if ((type & ModelScaleType.Child) != 0)
            {
                foreach (var child in children)
                {
                    child.SetScale(scale);
                }
            }
            // 粒子特效特殊修改
            if ((type & ModelScaleType.Particle) != 0)
            {
                foreach (var particle in particles)
                {
                    particle.SetScale(scale);
                }
            }
        }
        [Flags]
        enum ModelScaleType
        {
            None,
            Self = 1,
            Child = 2,
            Particle = 4,
        }
    }
}