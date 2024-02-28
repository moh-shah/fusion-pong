using Fusion;
using PhotoPong.Managers;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class LobbyPresenter : MonoBehaviour
    {
        private PongNetworkManager _pongNetworkManager;
        private void Start()
        {
            _pongNetworkManager = FindObjectOfType<PongNetworkManager>();
        }

        // Start is called before the first frame update
        void OnGUI()
        {
            if (_pongNetworkManager.runner == null)
            {
                if (GUI.Button(new Rect(0,0,200,40), "Host"))
                {
                    _pongNetworkManager.ConnectToGame(GameMode.Host);
                }
                if (GUI.Button(new Rect(0,40,200,40), "Join"))
                {
                    _pongNetworkManager.ConnectToGame(GameMode.Client);
                }
            }
        }
    }
}