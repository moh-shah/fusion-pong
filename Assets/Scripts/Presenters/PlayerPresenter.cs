using System.Collections;
using System.Linq;
using Fusion;
using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public abstract class PowerUp
    {
        public IEnumerator Activate()
        {
            OnActivated();
            yield return new WaitForSeconds(ActiveTime());
            OnDeActivated();
        }

        protected abstract void OnActivated();
        protected abstract float ActiveTime();
        protected abstract void OnDeActivated();
    }

    //pad size
    //multiple balls
    //ball speed
    public class FreeMovementPowerUp
    {
    }

    public class PlayerPresenter : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(OnScoreChanged))] public int Score { get; set; } = 0;
        [Networked] public bool PlayerReady { get; private set; }
        [Networked] public WorldDirection Side { get; private set; }

        private PaddlePresenter _playerPad;

        public void Setup(Transform parent, WorldDirection side)
        {
            this.Side = side;
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