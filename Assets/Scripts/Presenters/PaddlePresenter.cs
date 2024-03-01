using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;
using Fusion;

namespace PhotoPong.Presenters
{
    public class PaddlePresenter : NetworkBehaviour
    {
        public WorldDirection side;
        
        private Rigidbody2D _rb;
        private readonly Vector3 _up = Vector2.up * SpeedModifier;
        private readonly Vector3 _down = Vector2.up * SpeedModifier;
        private const float SpeedModifier = .2f;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public override void Spawned()
        {
            Debug.Log($"Paddle {side} Spawned");
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