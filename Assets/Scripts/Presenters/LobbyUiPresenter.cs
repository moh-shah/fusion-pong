using DG.Tweening;
using Fusion;
using PhotoPong.Managers;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class LobbyUiPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject loadingImage;
        [SerializeField] private GameObject buttonsParent;
        
        public void OnStartGameClicked(bool host)
        {
            var mode = host ? GameMode.Host : GameMode.Client;
            GameManager.Instance.ConnectToGame(mode);
            buttonsParent.SetActive(false);
            loadingImage.SetActive(true);
            loadingImage.transform.DORotate(new Vector3(0, 0, -360), 3, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}