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
        }
        
        
    }
}