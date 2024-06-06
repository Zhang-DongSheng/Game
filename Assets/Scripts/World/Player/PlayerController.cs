using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Game.Model
{
    public class PlayerController : RuntimeBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private LineRenderer arrow;

        [SerializeField] private float speed = 1;

        private Vector3 vector = Vector3.zero;

        private Vector3 position;

        protected override void OnAwake()
        {
            if (target == null)
                target = transform;
            position = transform.position;
        }

        protected override void OnUpdate(float delta)
        {
            vector.x = Input.GetAxisRaw("Horizontal") * delta * speed;

            vector.z = Input.GetAxisRaw("Vertical") * delta * speed;

            vector.y = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

            target.position += vector * speed * Time.deltaTime;
            // �������ǳ���ʹ��Vector3.Lerp���в�ֵ���㣬ʹ�ýǶȱ仯����ô��Ӳ
            target.forward = Vector3.Lerp(target.forward, vector, speed * Time.deltaTime);

            //arrow.SetPositions(target.position, )
        }
    }
}