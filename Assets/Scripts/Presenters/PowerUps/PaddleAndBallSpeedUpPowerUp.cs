using System.Linq;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class PaddleAndBallSpeedUpPowerUp : PowerUp
    {
        private Vector2 _ballInitialVelocity;
        private float _ballInitialForceAmplifier;
        
        public override bool CanActive(PlayerInput input)=> input.eKey;
        protected override float ActiveTime() => 8;

        protected override void OnActivated()
        {
            var balls = Object.FindObjectsOfType<BallPresenter>().ToList();
            var mainBall = balls.First(b => b.destroyWhenCollideWithGoal == false);
            _ballInitialVelocity = mainBall.rb.velocity;
            _ballInitialForceAmplifier = mainBall.forceAmplifier;
            foreach (var ball in balls)
            {
                ball.rb.velocity *= 1.5f;
                ball.forceAmplifier *= 1.5f;
            }

            var paddles = Object.FindObjectsOfType<PaddlePresenter>().ToList();
            foreach (var paddle in paddles)
                paddle.externalSpeedModifier = 1.5f;
        }

        protected override void OnDeActivated()
        {
            var balls = Object.FindObjectsOfType<BallPresenter>().ToList();
            foreach (var ball in balls)
            {
                ball.rb.velocity = _ballInitialVelocity;
                ball.forceAmplifier = _ballInitialForceAmplifier;
            }
            
            var paddles = Object.FindObjectsOfType<PaddlePresenter>().ToList();
            foreach (var paddle in paddles)
                paddle.externalSpeedModifier = 1;
        }
    }
}