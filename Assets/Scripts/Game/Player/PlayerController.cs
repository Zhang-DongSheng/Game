using UnityEngine;

namespace Game
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [SerializeField] private Transform m_target;

        [SerializeField] private Player m_player;

        private Vector2 vector = new Vector2();

        private void Start()
        {
            if (m_player == null)
            {
                InitPlayer();
            }
        }

        private void Update()
        {
            vector.x = Input.GetAxis("Horizontal") * 10f;

            vector.y = Input.GetAxis("Vertical") * 10f;

            if (vector != Vector2.zero)
            {
                Move(vector);
            }
        }

        public void InitPlayer()
        {
            Factory.Instance.Pop("MODEL ANIMATION", (value) =>
            {
                GameObject role = value as GameObject;

                role.transform.SetParent(m_target);

                role.transform.localPosition = Vector3.zero;

                Player = role.GetComponent<Player>();
            });
        }

        public void Move(Vector2 vector)
        {
            if (Vector2.zero.Equals(vector))
                return;

            float speed = Vector3.Distance(Vector2.zero, vector);

            Vector3 dir = vector.Vector2To3();

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

            m_target.rotation = Quaternion.Lerp(m_target.rotation, rotation, Time.deltaTime * PlayerConfig.Rotate);

            m_target.Translate(Vector3.forward * Time.deltaTime * speed);

            if (speed > 10)
            {
                Player.Run();
            }
            else
            {
                Player.Walk();
            }
        }

        public void Jump()
        {
            Player.Jump();
        }

        public void Crouch()
        {

        }

        public void Attack()
        {
            Player.Attack();
        }

        public void ReleaseSkill(int index)
        {
            Player.ReleaseSkill(index);
        }

        public Player Player
        {
            get
            {
                return m_player;
            }
            set
            {
                m_player = value;
            }
        }
    }
}