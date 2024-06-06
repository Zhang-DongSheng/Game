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
            // 更新主角朝向，使用Vector3.Lerp进行插值运算，使得角度变化不那么生硬
            target.forward = Vector3.Lerp(target.forward, vector, speed * Time.deltaTime);

            //arrow.SetPositions(target.position, )
        }
    }
}