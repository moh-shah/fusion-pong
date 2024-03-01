using System.Collections.Generic;
using PhotoPong.Models;
using System.Linq;
using UnityEngine;
using Fusion;

namespace PhotoPong.Presenters
{
    public class PaddlePresenter : NetworkBehaviour
    {
        public WorldDirection side;
        
        [HideInInspector] public Rigidbody2D rb;
        private readonly Vector3 _up = Vector2.up * SpeedModifier;
        private readonly Vector3 _down = Vector2.up * SpeedModifier;
        private const float SpeedModifier = .2f;
        private readonly List<PowerUp> _powerUps = new();
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public override void Spawned()
        {
            Debug.Log($"Paddle {side} Spawned");
            _powerUps.Add(new BiggerPaddlePowerUp().Setup(this));
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInput input))
            {
                if (input.upKey || input.downKey)
                {
                    var targetPos = transform.position;
                    if (input.upKey) targetPos += _up;
                    if (input.downKey) targetPos -= _down;
                    rb.MovePosition(targetPos);
                }

                var powerUpToActive = _powerUps.FirstOrDefault(p => !p.Used && p.CanActive(input));
                if (powerUpToActive != null)
                {
                    StartCoroutine(powerUpToActive.Activate());
                }
            }
        }
    }
}