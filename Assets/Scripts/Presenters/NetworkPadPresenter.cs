using System;
using System.Dynamic;
using Fusion;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class NetworkPadPresenter : NetworkBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInput input))
            {
                var targetPos = transform.position;
                if (input.up) targetPos += Vector3.up;
                if (input.down) targetPos -= Vector3.up;
                _rb.MovePosition(targetPos);
            }
        }
    }
}