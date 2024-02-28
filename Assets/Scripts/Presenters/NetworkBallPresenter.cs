using Fusion;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public class NetworkBallPresenter : NetworkBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = 0;
        }

        public void OnGameStarted()
        {
            _rb.AddForce(Vector2.right * 100, ForceMode2D.Force);
        }
    }
}