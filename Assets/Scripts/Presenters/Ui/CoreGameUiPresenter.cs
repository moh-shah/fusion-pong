using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;
using TMPro;

namespace PhotoPong.Presenters
{
    public class CoreGameUiPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject counterPanel;
        [SerializeField] private TextMeshProUGUI startGameCounter;
        [SerializeField] private TextMeshProUGUI leftPlayerScore;
        [SerializeField] private TextMeshProUGUI rightPlayerScore;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private GameObject corePanel;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI matchResult;

        private void Awake()
        {
            leftPlayerScore.text = rightPlayerScore.text = timer.text = "0";
            GameManager.Instance.Session.OnGameStarting += delegate(int i)
            {
                counterPanel.SetActive(true);
                startGameCounter.gameObject.SetActive(true);
                startGameCounter.text = $"{i}";
            };
            GameManager.Instance.Session.OnGameSessionStarted += OnGameSessionStarted;
        }

        private void OnGameSessionStarted()
        {
            counterPanel.SetActive(false);
            GameManager.Instance.Session.OnPlayerScoreChangedEvt += delegate(PlayerPresenter p)
            {
                switch (p.Side)
                {
                    case WorldDirection.Left:
                        leftPlayerScore.text = p.Score.ToString();
                        break;

                    case WorldDirection.Right:
                        rightPlayerScore.text = p.Score.ToString();
                        break;
                }
            };

            GameManager.Instance.Session.OnTimerElapsedEvt += delegate(int gameTime) { timer.text = $"{gameTime}"; };
            
            GameManager.Instance.Session.OnGameSessionEnded += delegate(MatchResults results)
            {
                corePanel.SetActive(false);
                resultPanel.SetActive(true);
                if (results.draw)
                    matchResult.text =
                        $"Draw ({results.winnerScore} - {results.loserScore})\n" +
                        $"in: {results.durationInSeconds} seconds";
                else
                    matchResult.text =
                        $"Winner: {results.winnerSide} ({results.winnerScore} - {results.loserScore})\n" +
                        $"in: {results.durationInSeconds} seconds";
            };
        }

        /*public void OnGoToLobbyClicked()
        {
            GameManager.Instance.BackToLobby();
        }*/
    }
}