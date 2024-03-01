using DG.Tweening;
using PhotoPong.Models;

namespace PhotoPong.Presenters
{
    public class BiggerPaddlePowerUp : PowerUp
    {
        protected override float ActiveTime() => 5;
        private float _initScale;
        protected override void OnActivated()
        {
            _initScale = ownerPaddle.rb.transform.localScale.y;
            ownerPaddle.rb.transform.DOScaleY(_initScale * 3, .5f);
        }
        
        protected override void OnDeActivated()
        {
            ownerPaddle.rb.transform.DOScaleY(_initScale, .5f);
        }

        public override bool CanActive(PlayerInput input)
        {
            return input.qKey;
        }
    }
}