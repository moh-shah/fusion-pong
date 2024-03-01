using System.Linq;
using Fusion;
using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class PlayerPresenter : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(OnScoreChanged))] public int Score { get; set; } = 0;
        [Networked] public bool PlayerReady { get; private set; }
        [Networked] public WorldDirection Side { get; private set; }

        private PaddlePresenter _playerPad;
        public void Setup(Transform parent, WorldDirection side)
        {
            Side = side;
            transform.SetParent(parent);
            GameManager.Instance.SetPlayer(this);
            GameManager.Instance.Session.OnGameSessionStarted += delegate
            {
                _playerPad = FindObjectsOfType<PaddlePresenter>().ToList().First(p => p.side == side);
                _playerPad.Object.AssignInputAuthority(Object.InputAuthority);
            };

            GameManager.Instance.Session.OnGameSessionEnded += delegate { _playerPad.Object.RemoveInputAuthority(); };

            PlayerReady = true;
        }
        
        public static void OnScoreChanged(Changed<PlayerPresenter> changed)
        {
            Debug.Log($"{changed.Behaviour.Side} player score changed.");
            GameManager.Instance.Session.PlayerScoreChanged(changed.Behaviour);
        }
    }
}