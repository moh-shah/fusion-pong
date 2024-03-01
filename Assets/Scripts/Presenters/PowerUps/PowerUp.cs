using System.Collections;
using PhotoPong.Models;
using UnityEngine;

namespace PhotoPong.Presenters
{
    public abstract class PowerUp
    {
        public bool Used { get; private set; }
        protected PaddlePresenter ownerPaddle;
        
        public PowerUp Setup(PaddlePresenter owner)
        {
            ownerPaddle = owner;
            return this;
        }
        
        public IEnumerator Activate()
        {
            if (Used)
                yield break;
            
            Used = true;
            OnActivated();
            Debug.Log($"powerup {GetType().Name} has been activated");
            yield return new WaitForSeconds(ActiveTime());
            OnDeActivated();
            Debug.Log($"powerup {GetType().Name} has been de-activated");
        }

        public abstract bool CanActive(PlayerInput input);
        protected abstract float ActiveTime();
        protected abstract void OnActivated();
        protected abstract void OnDeActivated();
    }
}