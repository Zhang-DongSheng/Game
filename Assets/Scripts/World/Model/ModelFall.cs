using Game.Attribute;
using System.Collections.Generic;
using UnityEngine;

namespace Game.World
{
    /// <summary>
    /// 模型倒地特效(禁止勾选静态)
    /// 最外层:设置Layer:Broken,添加碰撞盒,碰撞盒勾选Trigger
    /// 物理层:设置Layer:BrokenModel,添加刚体,刚体勾选isKinematic
    /// 表现层:一比一跟随物理层子节点表现
    /// </summary>
    public class ModelFall : MonoBehaviour
    {
        [FieldName("破碎表现系数")]
        [SerializeField] private float ratio = 30f;
        [FieldName("沉入地面时间")]
        [SerializeField] private float fade = 1f;
        [FieldName("碰撞高度偏移")]
        [SerializeField] private float height = 0f;
        [FieldName("动画展示时间")]
        [SerializeField] private float interval = 3f;
        [FieldName("动画表现精度")]
        [SerializeField] private float speed = 5f;
        [FieldName("物理层")]
        [SerializeField] private Transform physic;
        [FieldName("表现层")]
        [SerializeField] private Transform display;

        private float timer;

        private Vector3 direction;

        private bool into;

        private bool active, trigger;

        private readonly List<Rigidbody> children = new List<Rigidbody>();

        private readonly List<Pair<Transform>> pairs = new List<Pair<Transform>>();

        private void Awake()
        {
            trigger = false;

            into = false;

            active = true;

            children.AddRange(physic.GetComponentsInChildren<Rigidbody>());

            if (physic && display)
            {
                var pc = physic.GetComponentsInChildren<Transform>();

                var dc = display.GetComponentsInChildren<Transform>();

                var count = Mathf.Min(pc.Length, dc.Length);

                for (int i = 0; i < count; i++)
                {
                    pairs.Add(new Pair<Transform>()
                    {
                        x = pc[i],
                        y = dc[i]
                    });
                }
            }
        }

        private void Update()
        {
            if (active && trigger)
            {
                var delta = Time.deltaTime;

                timer += delta;

                if (timer > fade && !into)
                {
                    Fade();
                }

                if (timer > interval)
                {
                    Destroy();
                }
                Lerp(delta);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (trigger) return;

            trigger = true;

            if (other.attachedRigidbody != null && other.attachedRigidbody.velocity != Vector3.zero)
            {
                direction = other.attachedRigidbody.velocity.normalized;
            }
            else
            {
                direction = (other.transform.position - transform.position).normalized;
            }
            Fly(other.transform.position, direction);
        }

        private void Fly(Vector3 position, Vector3 direction)
        {
            position.y += height;

            int count = children.Count;

            for (int i = 0; i < count; i++)
            {
                children[i].isKinematic = false;
                children[i].AddForceAtPosition(direction * ratio, position);
            }
        }

        private void Fade()
        {
            into = true;

            var components = physic.GetComponentsInChildren<Collider>();

            foreach (var collider in components)
            {
                collider.isTrigger = true;
            }
        }

        private void Lerp(float delta)
        {
            int count = pairs.Count;

            for (int i = 0; i < count; i++)
            {
                pairs[i].y.position = Vector3.Lerp(pairs[i].y.position, pairs[i].x.position, delta * speed);

                pairs[i].y.rotation = Quaternion.Lerp(pairs[i].y.rotation, pairs[i].x.rotation, delta * speed);
            }
        }

        private void Destroy()
        {
            active = false;

            pairs.Clear();

            GameObject.Destroy(gameObject);
        }
    }
}