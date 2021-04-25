using UnityEngine;

namespace Deblue.LD48
{
    public class TeaStation : TakebleObjectContainer
    {
        public override bool CanReturn => _cupTaken && _player.TakenObject is CupOfTea;
        public override bool CanTake => !_cupTaken;
        protected override bool CanHighlight => !_cupTaken;

        [SerializeField] private CupOfTea _cup;
        [SerializeField] private SpritePair _withTea;
        [SerializeField] private SpritePair _withoutTea;

        private bool _cupTaken;

        public override ITakebleObject Take(Vector3 takePosition)
        {
            _cupTaken = true;
            return _cup;
        }

        public override void Return()
        {
            _cupTaken = false;
            _cup = (CupOfTea)_player.TakenObject;
        }

        protected override void Highlight()
        {
            Renderer.sprite = _cupTaken ? _withoutTea.Highlight : _withTea.Highlight;
        }

        protected override void StopHighlight()
        {
            Renderer.sprite = _cupTaken ? _withoutTea.Standart : _withTea.Standart;
        }
    }
}