using System;
using UnityEngine;

namespace Game.Model
{
    public class ModelScaleParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        [SerializeField] private float count = 10;

        [SerializeField] private float time = 10;

        [SerializeField] private Vector3 scale = Vector3.one;

        [SerializeField] private Axis axis;

        public void SetScale(Vector3 scale)
        {
            if (particle == null) return;

            ParticleSystem.ShapeModule shape = particle.shape;

            switch (axis)
            {
                case Axis.XY:
                    shape.scale = new Vector3(this.scale.x * scale.x, this.scale.y * scale.z, this.scale.z);
                    break;
                case Axis.XZ:
                    shape.scale = new Vector3(this.scale.x * scale.x, this.scale.y, this.scale.z * scale.z);
                    break;
                case Axis.YZ:
                    shape.scale = new Vector3(this.scale.x, this.scale.y * scale.x, this.scale.z * scale.z);
                    break;
                default:
                    shape.scale = new Vector3(this.scale.x * scale.x, this.scale.y * scale.y, this.scale.z * scale.z);
                    break;
            }
            ParticleSystem.MainModule main = particle.main;

            main.maxParticles = Convert.ToInt32(count * (scale.x * scale.z));

            ParticleSystem.EmissionModule emission = particle.emission;

            emission.rateOverTime = Convert.ToInt32(time * (scale.x * scale.z));
        }

        enum Axis
        { 
            XY,
            XZ,
            YZ,
            XYZ,
        }
    }
}