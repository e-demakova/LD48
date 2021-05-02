using UnityEngine;

using Deblue.Interactive;

namespace Deblue.LD48
{
    //TODO: Создать подкласс TakenObject с базовой реализацие всех методов и свойств.
    public class Tomato : TakebleObject
    {
        public override bool CanPut => _isTaken;
        public override bool CanTake => !_isTaken;
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