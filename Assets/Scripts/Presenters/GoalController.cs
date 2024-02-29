using System;
using PhotoPong.Managers;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class GoalController : MonoBehaviour
    {
        [SerializeField] private WorldDirection side;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"trigger entered: {other.name}");
            if (other.TryGetComponent(out NetworkBallPresenter ballPresenter))
            {
                PongNetworkManager.Instance.GoalScoredOnSide(side);
            }
        }
    }
}