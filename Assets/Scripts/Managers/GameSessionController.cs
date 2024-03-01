using System;
using System.Collections;
using Fusion;
using PhotoPong.Models;
using PhotoPong.Presenters;
using UnityEngine;

namespace PhotoPong.Managers
{
    public class GameSessionController : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(OnTimerElapsed))] 
        public int SessionTimer { get; set; }
        
        public event Action<int> OnGameStarting = delegate {  };
        public event Action OnGameSessionStarted = delegate {  };
        public event Action<MatchResults> OnGameSessionEnded = delegate {  };
        public event Action<int> OnTimerElapsedEvt = delegate {  }; 
        public event Action<PlayerPresenter> OnPlayerScoreChangedEvt = delegate {  };
        
        private const int MaxScore = 5;

        public override void Spawned()
        {
            Debug.Log($"Session spawned!");
            GameManager.Instance.Session = this;
            transform.SetParent(GameManager.Instance.transform);
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (Object.HasStateAuthority)
                StopCoroutine(TimerCountdown());
        }

        public void GoalDetectedOnSide(WorldDirection direction)
        {
            if (Object==null || !Object.HasStateAuthority)
                return;

            var scoredSide = direction.Opposite();
            var scoredPlayer = GameManager.Instance.GetPlayerBySide(scoredSide);
            scoredPlayer.Score++;
            Debug.Log($"goal detected on side: {direction} | increasing player {scoredPlayer.Side} score");
        }
        
        public void PlayerScoreChanged(PlayerPresenter playerPresenter)
        {
            OnPlayerScoreChangedEvt.Invoke(playerPresenter);
            if (Object.HasStateAuthority && playerPresenter.Score >= MaxScore)
            {
                var results = new MatchResults()
                {
                    winnerSide = playerPresenter.Side,
                    winnerScore = playerPresenter.Score,
                    loserScore = GameManager.Instance.GetPlayerBySide(playerPresenter.Side.Opposite()).Score
                };
                GameManager.Instance.EndGameSession(results);
            }
        }
        
        public IEnumerator StartGameSession()
        {
            yield return new WaitForEndOfFrame();
          
            Debug.Log($"players are ready. the game is about to begin.");
            for (var i = 3; i > 0; i--)
            {
                StartingGameSession_Rpc(i);
                yield return new WaitForSeconds(1);
            }

            StartGameSession_RPC();
        }

        #region RPCs
        
        [Rpc(sources: RpcSources.StateAuthority,targets: RpcTargets.All)]
        public void StartingGameSession_Rpc(int remainingTime)
        {
            Debug.Log($"StartGameSessionRpc");
            OnGameStarting.Invoke(remainingTime);
        }
        
        [Rpc(sources: RpcSources.StateAuthority,targets: RpcTargets.All)]
        public void EndGameSession_Rpc(MatchResults result)
        {
            Debug.Log($"EndGameSessionRpc");
            OnGameSessionEnded.Invoke(result);
        }

        [Rpc(sources: RpcSources.StateAuthority,targets: RpcTargets.All)]
        public void StartGameSession_RPC()
        {
            OnGameSessionStarted.Invoke();
            if (Object.HasStateAuthority)
                StartCoroutine(TimerCountdown());
        }

        #endregion
        
        private IEnumerator TimerCountdown()
        {
            var wait = new WaitForSeconds(1);
            while (true)
            {
                yield return wait;
                SessionTimer++;
            }
        }
        
        private static void OnTimerElapsed(Changed<GameSessionController> changed)
        {
            GameManager.Instance.Session.OnTimerElapsedEvt.Invoke(changed.Behaviour.SessionTimer);
        }
    }
}