using System;
using Fusion;
using PhotoPong.Managers;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class NetworkPlayerPresenter : NetworkBehaviour
    {
        [HideInInspector] public WorldDirection side;
        [Networked(OnChanged = nameof(OnScoreChanged))] public int Score { get; set; } = 0;
        private NetworkPadPresenter _playerPad;

        public NetworkPlayerPresenter Setup(
            NetworkPadPresenter playerPad,
            Transform parent,
            WorldDirection worldDirection
        )
        {
            side = worldDirection;
            _playerPad = playerPad;
            transform.SetParent(parent);
            return this;
        }
        
        public void OnGameStarted()
        {
            _playerPad.Object.AssignInputAuthority(Object.InputAuthority);
        }
        
        public static void OnScoreChanged(Changed<NetworkPlayerPresenter> changed)
        {
            PongNetworkManager.Instance.PlayerScoreChanged(changed.Behaviour);
        }
    }
}