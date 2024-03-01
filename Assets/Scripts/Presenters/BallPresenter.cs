using System;
using System.Collections;
using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;
using Fusion;

namespace PhotoPong.Presenters
{
    public class BallPresenter : NetworkBehaviour
    {
        private Rigidbody2D _rb;
        private int _forceAmplifier = 300;
        private System.Random _rnd;
        public override void Spawned()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = 0;
            Debug.Log($"ball spawned with state authority? {Object.HasStateAuthority}");
            if (Object.HasStateAuthority)
                _rnd = new System.Random(Object.Runner.TicksExecuted);
            
            GameManager.Instance.Session.OnGameSessionStarted += delegate
            {
                SubscribeToSessionEvents();
                AddRandomForce(WorldDirection.Right);
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
                StartCoroutine(ResetPosition(p.Side));
            };
        }

        private IEnumerator ResetPosition(WorldDirection requester)
        {
            transform.position = Vector3.zero;
            _rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(2);
            AddRandomForce(requester.Opposite());
        }
        

        private void AddRandomForce(WorldDirection direction)
        {
            Debug.Log($"ball wants random force. state authority? {Object.HasStateAuthority}");

            if (Object.HasStateAuthority == false)
                return;
            
            var force = direction == WorldDirection.Right ? Vector2.right : Vector2.left;
            force.y = _rnd.Next(-10, 10) / 10f;
            if (force.y == 0)
                force.y = direction == WorldDirection.Right ? .1f : -.1f;
            
            Debug.Log($"ball wants random force. force {force}");
            _rb.AddForce(force * _forceAmplifier, ForceMode2D.Force);
        }
    }
}