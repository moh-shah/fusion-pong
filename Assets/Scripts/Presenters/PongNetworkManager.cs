using System;
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
        [HideInInspector] public NetworkRunner runner;
        [SerializeField] private NetworkPrefabRef playerPrefab;
        
        private GameWorldPresenter _gameWorldPresenter;

        private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters =
            new Dictionary<PlayerRef, NetworkObject>();
     
        public async void StartGame(GameMode mode)
        {
            _gameWorldPresenter = FindObjectOfType<GameWorldPresenter>();
            runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
            
            var startGameResult = await runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
            Debug.Log($"game started: {startGameResult.Ok}");
        }
        
        //
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            var playerCount = runner.ActivePlayers.Count(); 
            Debug.Log($"Player joined: {playerRef.PlayerId} | player count: {playerCount}");
            if (playerCount > 2)
            {
                Debug.LogError($"Room is full... dont let anybody in!");
                return;
            }

            if (runner.IsServer)
            {
                var pos = playerCount == 1
                    ? _gameWorldPresenter.leftPlayerPosition.position
                    : _gameWorldPresenter.rightPlayerPosition.position;

                var networkObject = runner.Spawn(playerPrefab, pos, Quaternion.identity, playerRef);
                _spawnedCharacters.Add(playerRef, networkObject);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
        {
            Debug.Log($"Player left: {playerRef.PlayerId}");
            runner.Despawn(_spawnedCharacters[playerRef]);
            _spawnedCharacters.Remove(playerRef);
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

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log($"OnConnectedToServer");
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            Debug.Log($"OnDisconnectedFromServer");
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}