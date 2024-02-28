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
        private const float SpeedModifier = .2f;
        private Vector3 _up = Vector2.up * SpeedModifier;
        private Vector3 _down = Vector2.up * SpeedModifier;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInput input))
            {
                var targetPos = transform.position;
                if (input.up) targetPos += _up;
                if (input.down) targetPos -= _down;
                _rb.MovePosition(targetPos);
            }
        }
    }
}