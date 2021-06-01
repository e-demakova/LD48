using UnityEngine;

namespace Deblue.Interactive
{
    public class StandartTakebleObject : TakebleObject
    {
        public override bool CanPut => _isTaken;
        public override bool IsCanBeTaken => !_isTaken;

        [SerializeField] protected SpritePair _sprites;

        public sealed override void StopHighlight()
        {
            Renderer.sprite = _sprites.Standart;
        }

        public sealed override void Highlight()
        {
            Renderer.sprite = _sprites.Highlight;
        }

        public override bool CanHighlight(IObjectTaker taker)
        {
            return !_isTaken;
        }
    }
}