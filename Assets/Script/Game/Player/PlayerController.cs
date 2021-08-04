using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [SerializeField] private Camera m_camera;

        [SerializeField] private Transform m_target;

        private void Awake()
        {
            
        }

        private void LateUpdate()
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_target.position, Time.deltaTime * PlayerConfig.Follow);
        }

        public void InitPlayer()
        { 
            
        }

        public void OnMove(Vector2 vector)
        {
            if (Vector2.zero.Equals(vector))
                return;

            Vector3 dir = new Vector3(vector.x, 0, vector.y);

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

            m_target.rotation = Quaternion.Lerp(m_target.rotation, rotation, Time.fixedDeltaTime * PlayerConfig.Rotate);

            m_target.Translate(Vector3.forward * Time.deltaTime * PlayerConfig.Speed);
        }

        public void OnAttack()
        { 
            
        }

        public void OnJump()
        {

        }

        public void OnCrouch()
        {

        }

        public void OnReleaseSkill(int index)
        { 
            
        }

        public Player player { get; set; }
    }
}
