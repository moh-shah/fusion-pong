using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using PhotoPong.Models;
using PhotoPong.Presenters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotoPong.Managers
{
    public class PongNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public event Action<int> OnGameStarting = delegate {  };
        public event Action OnGameStarted = delegate {  };
        
        [SerializeField] private NetworkPlayerPresenter playerPresenterPrefab;
        [SerializeField] private NetworkBallPresenter networkBallPresenter;
        [SerializeField] private NetworkPadPresenter leftPad;
        [SerializeField] private NetworkPadPresenter rightPad;

        //should I have a session controller?
        
        private NetworkRunner _runner;
        private INetworkSceneManager _sceneManager;
        private readonly Dictionary<PlayerRef, NetworkPlayerPresenter> _spawnedPlayers = new();

        public async void ConnectToGame(GameMode mode)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            var startGameResult = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "PhotoPong_01",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                PlayerCount = 2
            });
            Debug.Log($"game started: {startGameResult.Ok}");
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log($"players are ready. the game is about to begin.");
            for (var i = 3; i > 0; i--)
            {
                OnGameStarting.Invoke(i);
                yield return new WaitForSeconds(1);
            }
            
            networkBallPresenter.OnGameStarted();
            foreach (var (_, networkPlayerPresenter) in _spawnedPlayers)
            {
                networkPlayerPresenter.OnGameStarted();
            }
            
            OnGameStarted.Invoke();
        }
        
        //network runner callbacks
#region Network Runner Callbacks
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            var playerCount = runner.ActivePlayers.Count(); 
            Debug.Log($"Player joined: {playerRef.PlayerId} | player count: {playerCount}");
            
            var padToAssign = playerCount == 1 ? leftPad : rightPad;
            if (runner.IsServer)
            {
                var networkObject = runner.Spawn(
                    prefab: playerPresenterPrefab,
                    inputAuthority: playerRef
                ).Setup(padToAssign,transform);
                _spawnedPlayers.Add(playerRef, networkObject);
            }

            if (playerCount == 2)
            {
                StartCoroutine(StartGame());
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
        {
            Debug.Log($"Player left: {playerRef.PlayerId}");
            runner.Despawn(_spawnedPlayers[playerRef].Object);
            _spawnedPlayers.Remove(playerRef);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var newInput = new PlayerInput()
            {
                up = Input.GetKey(KeyCode.UpArrow),
                down = Input.GetKey(KeyCode.DownArrow),
            };
            input.Set(newInput);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log($"OnConnectedToServer");
        }
        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            Debug.Log($"OnDisconnectedFromServer");
        }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,byte[] token){}
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}
        public void OnSceneLoadDone(NetworkRunner runner){}
        public void OnSceneLoadStart(NetworkRunner runner){}
#endregion
    }
}
