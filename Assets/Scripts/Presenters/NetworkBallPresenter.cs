using System;
using Fusion;
using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PhotoPong.Presenters
{
    public class NetworkBallPresenter : NetworkBehaviour
    {
        private Rigidbody2D _rb;
        private int _forceAmplifier = 300;
 
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = 0;

            GameManager.Instance.Session.OnGameSessionStarted += delegate
            {
                SubscribeToSessionEvents();
                _rb.AddForce(RandomForce(WorldDirection.Right), ForceMode2D.Force);
            };
            
            GameManager.Instance.Session.OnGameSessionEnded += delegate
            {
                gameObject.SetActive(false);
            };

        }

        private void SubscribeToSessionEvents()
        {
            GameManager.Instance.Session.OnPlayerScoreChangedEvt += delegate(PlayerPresenter p)
            {
                ResetPosition(p.side);
            };
        }

        private void ResetPosition(WorldDirection requester)
        {
            transform.position = Vector3.zero;
            _rb.velocity = Vector2.zero;
            var force = RandomForce(requester.Opposite());
            _rb.AddForce(force, ForceMode2D.Force);
        }

        private Vector2 RandomForce(WorldDirection direction)
        {
            var force = direction == WorldDirection.Right ? Vector2.right : Vector2.left;
            force.y = Random.Range(-1, 1);
            return force * _forceAmplifier;
        }
    }
}