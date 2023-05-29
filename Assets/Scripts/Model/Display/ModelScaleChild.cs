using UnityEngine;

namespace Game.Model
{
    public class ModelScaleChild : MonoBehaviour
    {
        [SerializeField] private Transform m_target;

        [SerializeField] private Vector3 scale;

        [SerializeField] private Axis axis;

        private Vector3 m_scale = Vector3.one;

        public void SetScale(Vector3 scale)
        {
            switch (axis)
            {
                case Axis.XY:
                    m_scale = new Vector3(this.scale.x * scale.x, this.scale.y * scale.z, this.scale.z);
                    break;
                case Axis.XZ:
                    m_scale = new Vector3(this.scale.x * scale.x, this.scale.y, this.scale.z * scale.z);
                    break;
                case Axis.YZ:
                    m_scale = new Vector3(this.scale.x, this.scale.y * scale.x, this.scale.z * scale.z);
                    break;
                default:
                    m_scale = new Vector3(this.scale.x * scale.x, this.scale.y * scale.y, this.scale.z * scale.z);
                    break;
            }
            m_target.localScale = m_scale;
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