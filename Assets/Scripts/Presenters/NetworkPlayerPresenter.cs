using Fusion;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class NetworkPlayerPresenter : NetworkBehaviour
    {
        private NetworkPadPresenter _playerPad;

        public NetworkPlayerPresenter Setup(NetworkPadPresenter playerPad,Transform parent)
        {
            _playerPad = playerPad;
            transform.SetParent(parent);
            return this;
        }
        
        public void OnGameStarted()
        {
            _playerPad.Object.AssignInputAuthority(Object.InputAuthority);
        }
    }
}