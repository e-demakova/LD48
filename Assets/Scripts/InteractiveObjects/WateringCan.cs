using UnityEngine;

namespace Deblue.LD48
{
    public class WateringCan : TakebleObject
    {
        public override bool CanPut => _isTaken;
        public override bool CanTake => CanHighlight;
        protected override bool CanHighlight => !_isTaken;

        [SerializeField] protected SpritePair _sprites;

        protected sealed override void StopHighlight()
        {
            Renderer.sprite = _sprites.Standart;
        }

        protected sealed override void Highlight()
        {
            Renderer.sprite = _sprites.Highlight;
        }
    }
}