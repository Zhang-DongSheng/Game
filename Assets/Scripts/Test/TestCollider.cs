using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Test
{
    internal class TestCollider : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;

        [SerializeField] private Vector3 vector;

        [SerializeField] private Vector3 position;

        [SerializeField] private bool move;

        private void Update()
        {
            vector.x = Input.GetAxis("Horizontal");

            vector.z = Input.GetAxis("Vertical");

            rigidbody.AddForce(vector, ForceMode.Force);
        }

        private void FixedUpdate()
        {
            if (!move) return;

            rigidbody.MovePosition(position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debuger.LogError(Author.Test, "Enter");
        }

        private void OnCollisionStay(Collision collision)
        {
            Debuger.Log(Author.Test, "Stay");
        }

        private void OnCollisionExit(Collision collision)
        {
            Debuger.LogWarning(Author.Test, "Exit");
        }
    }
}
