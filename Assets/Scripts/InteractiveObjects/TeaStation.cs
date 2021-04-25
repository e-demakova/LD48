using UnityEngine;

namespace Deblue.LD48
{
    public class TeaStation : TakebleObjectContainer
    {
        public override bool CanReturn => _cupTaken && _player.TakenObject is CupOfTea;
        public override bool CanTake => !_cupTaken;
        protected override bool CanHighlight => CanTake || CanReturn;

        [SerializeField] private CupOfTea _cup;
        [SerializeField] private SpritePair _withTea;
        [SerializeField] private SpritePair _withoutTea;

        private bool _cupTaken;

        public override TakebleObject Take()
        {
            _cupTaken = true;
            _cup.gameObject.SetActive(true);
            Renderer.sprite = _withoutTea.Highlight;
            return _cup;
        }

        public override void Return()
        {
            _cupTaken = false;
            _cup.transform.SetParent(transform);
            _cup.gameObject.SetActive(false);
            Renderer.sprite = _withTea.Highlight;
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