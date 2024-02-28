using DG.Tweening;
using Fusion;
using PhotoPong.Managers;
using TMPro;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class UiPresenter : MonoBehaviour
    {
        //lobby
        [SerializeField] private TextMeshProUGUI startGameCounter;
        [SerializeField] private GameObject lobbyUiParent;
        [SerializeField] private GameObject startHostBtn;
        [SerializeField] private GameObject joinGameBtn;
        [SerializeField] private GameObject loadingImage;
        
        [Space]
        //in-game
        [SerializeField] private GameObject coreUiParent;
        [SerializeField] private TextMeshProUGUI leftPlayerScore;
        [SerializeField] private TextMeshProUGUI rightPlayerScore;

        private PongNetworkManager _pongNetworkManager;

        private void Awake()
        {
            _pongNetworkManager = FindObjectOfType<PongNetworkManager>();
            _pongNetworkManager.OnGameStarting += delegate(int i)
            {
                startGameCounter.gameObject.SetActive(true);
                loadingImage.gameObject.SetActive(false);
                startGameCounter.text = $"{i}";
            };

            _pongNetworkManager.OnGameStarted += () =>
            {
                startGameCounter.gameObject.SetActive(false);
                lobbyUiParent.SetActive(false);
                coreUiParent.SetActive(true);
            };
        }
        
        public void OnStartGameClicked(bool host)
        {
            var mode = host ? GameMode.Host : GameMode.Client;
            _pongNetworkManager.ConnectToGame(mode);
            startHostBtn.SetActive(false);
            joinGameBtn.SetActive(false);
            loadingImage.SetActive(true);
            loadingImage.transform.DORotate(new Vector3(0, 0, -360), 3, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}