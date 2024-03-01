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
    public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
    {
        public static GameManager Instance { get; private set; }
        public GameSessionController Session { get; set; }
        
        [SerializeField] private PlayerPresenter playerPresenterPrefab;
        [SerializeField] private GameSessionController sessionControllerPrefab;
        
        private NetworkRunner _runner;
        private NetworkSceneManagerBase _sceneManager;
        private List<PlayerPresenter> _spawnedPlayers = new();
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
            
            _sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            DontDestroyOnLoad(gameObject);
        }

        public void SetPlayer(PlayerPresenter playerPresenter) => _spawnedPlayers.Add(playerPresenter);
        public PlayerPresenter GetPlayerBySide(WorldDirection side) => _spawnedPlayers.First(p => p.Side == side);

        public async void ConnectToGame(GameMode mode)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            var startGameResult = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "PhotoPong_01",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = _sceneManager,
                PlayerCount = 2
            });

            Debug.Log($"start game result: {startGameResult.Ok}");
        }
        
        /*public void BackToLobby()
        {
            Debug.Log("BackToLobby");
            if (_runner.IsServer)
            {
                foreach (var player in _spawnedPlayers)
                    _runner.Despawn(player.Object);

                _runner.Despawn(Session.Object);
                Session = null;
                _spawnedPlayers.Clear();
                _runner.Shutdown();
            }

            SceneManager.LoadScene("Lobby");
        }*/
        
        //network runner callbacks
#region Network Runner Callbacks
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            if (!runner.IsServer)
                return;

            if (Session == null)
            {
                runner.Spawn(sessionControllerPrefab);
            }
            
            var playerCount = runner.ActivePlayers.Count();
            Debug.Log($"Player joined: {playerRef.PlayerId} | player count: {playerCount}");

            var side = playerCount == 1 ? WorldDirection.Left : WorldDirection.Right;
            runner.Spawn(
                prefab: playerPresenterPrefab,
                inputAuthority: playerRef
            ).Setup(transform, side);

            if (playerCount == 2)
            {
                StartCoroutine(GoToGameSceneIfBothPlayersAreReady());
            }
        }

        private IEnumerator GoToGameSceneIfBothPlayersAreReady()
        {
            var wait = new WaitForSeconds(1);
            while (true)
            {
                yield return wait;
                if (_spawnedPlayers.All(p=>p.PlayerReady))
                {
                    break;
                }
            }
            
            _sceneManager.Runner.SetActiveScene("Game");
            StartCoroutine(Session.StartGameSession());
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
        {
            Debug.Log($"Player left: {playerRef.PlayerId}");
            runner.Despawn(_spawnedPlayers[playerRef].Object);
            _spawnedPlayers = _spawnedPlayers.Where(p => p.Object.InputAuthority != playerRef).ToList();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var newInput = new PlayerInput()
            {
                upKey = Input.GetKey(KeyCode.UpArrow),
                downKey = Input.GetKey(KeyCode.DownArrow),
                qKey = Input.GetKey(KeyCode.Q),
                wKey = Input.GetKey(KeyCode.W),
                eKey = Input.GetKey(KeyCode.E),
            };
            input.Set(newInput);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
        public void OnConnectedToServer(NetworkRunner runner){}

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            Debug.LogError("OnDisconnectedFromServer");
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.LogError("OnShutdown");
        }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,byte[] token){}

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.LogError("OnConnectFailed");
        }
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
