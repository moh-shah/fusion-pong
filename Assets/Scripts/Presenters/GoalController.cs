using PhotoPong.Managers;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class GoalController : MonoBehaviour
    {
        [SerializeField] private WorldDirection side;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out NetworkBallPresenter _))
            {
                GameManager.Instance.Session.GoalDetectedOnSide(side);
            }
        }
    }
}