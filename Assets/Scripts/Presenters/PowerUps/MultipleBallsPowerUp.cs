using System.Collections.Generic;
using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class MultipleBallsPowerUp : PowerUp
    {
        private List<BallPresenter> _addedBalls = new();
        public override bool CanActive(PlayerInput input) => input.wKey;
        protected override float ActiveTime() => 1;
        private const int ExtraBallsToSpawn = 2;
        protected override void OnActivated()
        {
            if (ownerPaddle.HasStateAuthority == false)
                return;
            
            var mainBall = Object.FindObjectOfType<BallPresenter>();
            var mainBallPosition = mainBall.transform.position;

            for (var i = 0; i < ExtraBallsToSpawn; i++)
            {
                var ball = GameManager.Instance.Session.Runner.Spawn(GameManager.Instance.ballPrefab, mainBallPosition);
                ball.AddRandomForce(ownerPaddle.side.Opposite());
                ball.destroyWhenCollideWithGoal = true;
                _addedBalls.Add(ball);
            }
        }

        protected override void OnDeActivated()
        {
            //
        }
    }
}