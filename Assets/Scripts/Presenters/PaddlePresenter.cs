using System.Collections.Generic;
using PhotoPong.Models;
using System.Linq;
using UnityEngine;
using Fusion;

namespace PhotoPong.Presenters
{
    public class PaddlePresenter : NetworkBehaviour
    {
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public float externalSpeedModifier = 1;
        public WorldDirection side;
        
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
            Debug.Log($"{side} paddle spawned");
            _powerUps.Add(new BiggerPaddlePowerUp().Setup(this));
            _powerUps.Add(new MultipleBallsPowerUp().Setup(this));
            _powerUps.Add(new PaddleAndBallSpeedUpPowerUp().Setup(this));
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInput input))
            {
                if (input.upKey || input.downKey)
                {
                    var targetPos = transform.position;
                    if (input.upKey) targetPos += _up * externalSpeedModifier;
                    if (input.downKey) targetPos -= _down * externalSpeedModifier;
                    rb.velocity = Vector2.zero;
                    rb.MovePosition(targetPos);
                }

                var powerUpToActive = _powerUps.FirstOrDefault(p => !p.Used && p.CanActive(input));
                if (powerUpToActive != null)
                    StartCoroutine(powerUpToActive.Activate());
            }
        }
    }
}