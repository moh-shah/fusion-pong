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
            yield return new WaitForSeconds(ActiveTime());
            OnDeActivated();
        }

        public abstract bool CanActive(PlayerInput input);
        protected abstract float ActiveTime();
        protected abstract void OnActivated();
        protected abstract void OnDeActivated();
    }
    
    //pad size
    //multiple balls
    //ball speed
    public class FreeMovementPowerUp
    {
    }

}