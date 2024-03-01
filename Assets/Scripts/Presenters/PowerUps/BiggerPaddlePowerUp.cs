using DG.Tweening;
using PhotoPong.Models;

namespace PhotoPong.Presenters
{
    public class BiggerPaddlePowerUp : PowerUp
    {
        private float _initScale;
        
        public override bool CanActive(PlayerInput input) => input.qKey;
        protected override float ActiveTime() => 5;
        protected override void OnActivated()
        {
            _initScale = ownerPaddle.rb.transform.localScale.y;
            ownerPaddle.rb.transform.DOScaleY(_initScale * 3, .5f);
        }
        
        protected override void OnDeActivated()
        {
            ownerPaddle.rb.transform.DOScaleY(_initScale, .5f);
        }
    }
}