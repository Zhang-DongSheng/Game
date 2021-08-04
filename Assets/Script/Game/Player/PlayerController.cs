using UnityEngine;

namespace Game
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [SerializeField] private Transform m_camera;

        [SerializeField] private Transform m_follow;

        [SerializeField] private Transform m_target;

        private void Start()
        {
            InitPlayer();
        }

        private void LateUpdate()
        {
            m_camera.position = Vector3.Lerp(m_camera.position, m_follow.position, Time.deltaTime * PlayerConfig.Follow);
        }

        public void InitPlayer()
        {
            GameObject role = Factory.Instance.Pop("MODEL ANIMATION") as GameObject;

            role.transform.SetParent(m_target);

            role.transform.localPosition = Vector3.zero;

            player = role.GetComponent<Player>();
        }

        public void Move(Vector2 vector)
        {
            if (Vector2.zero.Equals(vector))
                return;

            float speed = Vector3.Distance(Vector2.zero, vector);

            Vector3 dir = new Vector3(vector.x, 0, vector.y);

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

            m_target.rotation = Quaternion.Lerp(m_target.rotation, rotation, Time.fixedDeltaTime * PlayerConfig.Rotate);

            m_target.Translate(Vector3.forward * Time.deltaTime * speed);

            if (speed > 10)
            {
                player.Run();
            }
            else
            {
                player.Walk();
            }
        }

        public void Jump()
        {
            player.Jump();
        }

        public void Crouch()
        {

        }

        public void Attack()
        {
            player.Attack();
        }

        public void ReleaseSkill(int index)
        {
            player.ReleaseSkill(index);
        }

        public Player player { get; set; }
    }
}