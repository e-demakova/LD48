using UnityEngine;

namespace Deblue.LD48
{
    public class CupOfTea : TakebleObject
    {
        [SerializeField] protected SpritePair _sprites;

        public override bool CanPut => throw new System.NotImplementedException();

        public override bool CanTake => throw new System.NotImplementedException();

        protected override bool CanHighlight => throw new System.NotImplementedException();

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